using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExtensionsPack.Core;
using TestShopApplication.Dal.Models;
using TestShopApplication.Dal.Repositories;
using TestShopApplication.Shared.ApiModels;

namespace TestShopApplication.Api.Services
{
    public class OrdersService
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly IOrderRepository _orderRepository;
        public OrdersService(IOrdersRepository ordersRepository,
            IOrderRepository orderRepository)
        {
            _ordersRepository = ordersRepository;
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<Order>> GetAll(Guid userId)
        {
            return await _ordersRepository.GetAll(userId);
        }
        
        public async Task<IEnumerable<OrderItemPresentation>> GetAllItems(Guid orderId)
        {
            var items = await _orderRepository.GetAll(orderId);
            return items.Select(i => new OrderItemPresentation
            {
                ItemId = Guid.Parse(i.ItemId),
                Name = i.Description,
                Price = i.Price,
                Quantity = i.Quantity
            });
        }

        public async Task<Guid> CreateOrder(Guid userId, IEnumerable<OrderItemPresentation> items)
        {
            var order = new Order
            {
                OrderId = Guid.NewGuid().ToString(),
                CreatedTimestamp = DateTime.Now.ToUnixUtcTimeStamp(),
                Price = items.Sum(i => i.Price * i.Quantity),
                Status = OrderStatus.Created.ToString()
            };

            var orderItems = items.Select(i => new OrderItemDto
            {
                OrderId = order.OrderId,
                ItemId = i.ItemId.ToString(),
                Description = i.Description,
                Name = i.Name,
                Price = i.Price,
                Quantity = i.Quantity
            });

            return await _orderRepository.CreateOrder(order, userId, orderItems);
        }

        public async Task<Response<bool>> CancelOrder(Guid orderId)
        {
            var result = await _orderRepository.UpdateOrderStatus(orderId, OrderStatus.Cancelled);
            return new Response<bool>
            {
                Success = result
            };
        }
    }
}