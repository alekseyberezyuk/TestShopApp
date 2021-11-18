using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using TestShopApplication.Dal.Common;

namespace TestShopApplication.Dal.Repositories
{
    public class ItemsRepository : IItemsRepository
    {
        private string ConnectionString { get; }

        public ItemsRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }
        
        public async Task<IEnumerable<Item>> GetAll(int categoryId)
        {
            string whereClause = categoryId > 0 
                ? $"WHERE [items].category_id={categoryId}" 
                : string.Empty;
            var query =
                $"SELECT item_id as itemId, name, description, price, [items].category_id as categoryId, category_name as categoryName FROM [items] " +
                "LEFT JOIN [item_categories] ON [items].category_id = [item_categories].category_id " +
                whereClause;
            using var connection = new SqlConnection(ConnectionString);
            var results = await connection.QueryAsync<Item>(query);
            return results;
        }

        public async Task<Item> GetById(Guid itemId)
        {
            string query = $"SELECT item_id as ItemId, name, description, price, i.category_id as categoryId, category_name as categoryName FROM [items] i " +
                           $"LEFT JOIN [item_categories] ic ON i.category_id = ic.category_id " +
                           $"WHERE item_id='{itemId}'";
            using var connection = new SqlConnection(ConnectionString);
            var result = await connection.QuerySingleAsync<Item>(query);
            return result;
        }

        public async Task<bool> TryAdd(ItemWithCategory item)
        {
            var request = $"INSERT INTO [items] (item_id, name, description, price, category_id)" +
                          $"VALUES('{item.ItemId}','{item.Name}','{item.Description}',{item.Price},{item.CategoryId})";
            using var connection = new SqlConnection(ConnectionString);
            var result = await connection.ExecuteAsync(request);
            return result >= 0;
        }

        public async Task<bool> TryUpdate(ItemWithCategory item)
        {
            var request = $"UPDATE [items] " +
                          $"SET name='{item.Name}', description='{item.Description}', " +
                          $"price={item.Price}, category_id={item.CategoryId}" +
                          $"WHERE item_id='{item.ItemId}'";
            using var connection = new SqlConnection(ConnectionString);
            var result = await connection.ExecuteAsync(request);
            return result >= 0;
        }

        public async Task<bool> TryDelete(Guid itemId)
        {
            var request = $"DELETE FROM [items]"+
                          $"WHERE item_id='{itemId}'";
            using var connection = new SqlConnection(ConnectionString);
            var result = await connection.ExecuteAsync(request);
            return result >= 0;
        }

        public async Task<bool> Exists(string name)
        {
            var request = $"SELECT count(*) FROM [items]" +
                          $"WHERE name='{name}'";
            using var connection = new SqlConnection(ConnectionString);
            var result = await connection.QuerySingleAsync<int>(request);
            return result > 0;
        }
    }
}