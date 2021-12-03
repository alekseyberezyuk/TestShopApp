using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using TestShopApplication.Dal.Models;

namespace TestShopApplication.Dal.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private string ConnectionString { get; }

        public OrderRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public async Task<IEnumerable<OrderItemDto>> GetAll(Guid orderId)
        {
            var request = $"SELECT [order_content].item_id as itemId, name, description, price, quantity " +
                          $"FROM [order_content] " +
                          $"LEFT JOIN [items] ON [order_content].item_id = [items].item_id " +
                          $"WHERE order_id = @orderId";
            using var connection = new SqlConnection(ConnectionString);
            var results = await connection.QueryAsync<OrderItemDto>(request, 
                new {orderId});
            return results;
        }

        public async Task<Guid> CreateOrder(Order order, Guid userId, IEnumerable<OrderItemDto> items)
        {
            var insertOrderRequest = $"INSERT INTO [orders](order_id, user_id, price, status, created_at)" +
                                     $"VALUES(@orderId,@userId,@price,@status,@createdAt)";
            using var connection = new SqlConnection(ConnectionString);
            var orderCreationResult = await connection.ExecuteAsync(insertOrderRequest, new
            {
                orderId = order.OrderId,
                userId,
                price = order.Price,
                status = (int)order.Status,
                createdAt = order.CreatedAt
            });
            if (orderCreationResult == 0)
                return Guid.Empty;
            var insertOrderItemsRequest = $"INSERT INTO [order_content](order_id, item_id, quantity) " +
                                          $"VALUES (@orderId, @itemId, @quantity)";
            var itemsCreationResult = await connection.ExecuteAsync(insertOrderItemsRequest, items.Select(item => new
            {
                orderId = order.OrderId,
                itemId = item.ItemId,
                quantity = item.Quantity
            }));
            
            if(itemsCreationResult == 0)
                return Guid.Empty;
            
            return order.OrderId;
        }

        public async Task<bool> UpdateOrderStatus(Guid orderId, OrderStatus status)
        {
            var request = $"UPDATE [orders] " +
                          $"SET status=@status " +
                          $"WHERE order_id=@orderId";

            using var connection = new SqlConnection(ConnectionString);
            var result = await connection.ExecuteAsync(request, new {orderId, status});
            return result > 0;
        }
    }
}