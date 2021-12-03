using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestShopApplication.Dal.Models;

namespace TestShopApplication.Dal.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<OrderItemDto>> GetAll(Guid orderId);
        Task<Guid> CreateOrder(Order order, Guid userId, IEnumerable<OrderItemDto> items);
        Task<bool> UpdateOrderStatus(Guid orderId, OrderStatus status);
    }
}