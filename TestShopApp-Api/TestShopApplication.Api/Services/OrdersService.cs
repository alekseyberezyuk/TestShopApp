using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestShopApplication.Api.Models;
using TestShopApplication.Dal.Common;
using TestShopApplication.Dal.Repositories;

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
        
        public async Task<IEnumerable<OrderItem>> GetAllItems(Guid orderId)
        {
            var items = await _orderRepository.GetAll(orderId);
            return items.Select(i => new OrderItem
            {
                ItemId = i.ItemId,
                Name = i.Description,
                Price = i.Price,
                Quantity = i.Quantity
            });
        }

        public async Task<Guid> CreateOrder(Guid userId, IEnumerable<OrderItem> items)
        {
            var order = new Order
            {
                OrderId = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                Price = items.Sum(i => i.Price),
                Status = OrderStatus.Created
            };

            var orderItems = items.Select(i => new OrderItemDto
            {
                OrderId = order.OrderId,
                ItemId = i.ItemId,
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