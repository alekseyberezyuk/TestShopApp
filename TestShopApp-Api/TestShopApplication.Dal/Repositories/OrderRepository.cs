using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Dapper;
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
            var request = $"SELECT [order_contents].item_id as itemId, name, description, price, quantity " +
                          $"FROM [order_contents] " +
                          $"LEFT JOIN [items] ON [order_contents].item_id = [items].item_id " +
                          $"WHERE order_id = @orderId";
            using var connection = new SqliteConnection(ConnectionString);
            var results = await connection.QueryAsync<OrderItemDto>(request, 
                new
                {
                    orderId = orderId.ToString()
                });
            return results;
        }

        public async Task<Guid> CreateOrder(Order order, Guid userId, IEnumerable<OrderItemDto> items)
        {
            var insertOrderRequest = $"INSERT INTO [orders] (order_id, user_id, price, status, created_timestamp) " +
                                     $"VALUES (@orderId,@userId,@price,@status,@createdTimestamp)";
            using var connection = new SqliteConnection(ConnectionString);
            
            bool isSuccess = await connection.ExecuteAsync(insertOrderRequest, new
            {
                @orderId = order.OrderId.ToString(),
                @userId = userId.ToString(),
                @price = order.Price,
                @status = order.Status,
                @createdTimestamp = order.CreatedTimestamp
            }) > 0;
            if (isSuccess)
            {
                var insertOrderItemsRequest = $"INSERT INTO [order_contents] (order_id, item_id, quantity) " +
                                              $"VALUES (@orderId, @itemId, @quantity)";
                isSuccess = await connection.ExecuteAsync(insertOrderItemsRequest, items.Select(item => new
                {
                    @orderId = order.OrderId.ToString(),
                    @itemId = item.ItemId,
                    @quantity = item.Quantity
                })) > 0;
            }
            return isSuccess ? Guid.Parse(order.OrderId) : Guid.Empty;
        }

        public async Task<bool> UpdateOrderStatus(Guid orderId, OrderStatus status)
        {
            var request = $"UPDATE [orders] " +
                          $"SET status=@status " +
                          $"WHERE order_id=@orderId";

            using var connection = new SqliteConnection(ConnectionString);
            var result = await connection.ExecuteAsync(request, new
            {
                orderId = orderId.ToString(), 
                status = status.ToString()
            });
            return result > 0;
        }
    }
}