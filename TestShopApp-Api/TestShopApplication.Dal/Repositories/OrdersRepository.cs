using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using TestShopApplication.Dal.Common;

namespace TestShopApplication.Dal.Repositories
{
    public class OrdersRepository: IOrdersRepository
    {
        private string ConnectionString { get; }
        public OrdersRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }
        
        public Task<IEnumerable<Order>> GetAll(Guid userId)
        {
            var request = $"SELECT order_id as orderId, price, status, created_at as createdAt " +
                          $"FROM [orders] " +
                          $"WHERE user_id='{userId}'";
            using var connection = new SqlConnection(ConnectionString);
            var result = connection.QueryAsync<Order>(request);
            return result;
        }
    }
}