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
        
        public async ValueTask<IEnumerable<Item>> GetAll(int categoryId)
        {
            string whereClause = categoryId > 0 
                ? $"WHERE [items].category_id=@categoryId" 
                : string.Empty;
            var query =
                $"SELECT item_id as itemId, name, description, price, [items].category_id as categoryId, category_name as categoryName FROM [items] " +
                "LEFT JOIN [item_categories] ON [items].category_id = [item_categories].category_id " +
                whereClause;
            using var connection = new SqlConnection(ConnectionString);
            var results = await connection.QueryAsync<Item>(query, new {categoryId});
            return results;
        }

        public async ValueTask<Item> GetById(Guid itemId)
        {
            string query = $"SELECT item_id as ItemId, name, description, price, i.category_id as categoryId, category_name as categoryName FROM [items] i " +
                           $"LEFT JOIN [item_categories] ic ON i.category_id = ic.category_id " +
                           $"WHERE item_id=@itemId";
            using var connection = new SqlConnection(ConnectionString);
            var result = await connection.QuerySingleAsync<Item>(query, new {itemId});
            return result;
        }

        public async ValueTask<bool> TryAdd(ItemWithCategory item)
        {
            var request = $"INSERT INTO [items] (item_id, name, description, price, category_id)" +
                          $"VALUES(@itemId, @name, @description, @price, @categoryId)";
            using var connection = new SqlConnection(ConnectionString);
            var result = await connection.ExecuteAsync(request, new
            {
                itemId = item.ItemId,
                name = item.Name,
                description = item.Description,
                price = item.Price,
                categoryId = item.CategoryId
            });
            return result >= 0;
        }

        public async ValueTask<bool> TryUpdate(ItemWithCategory item)
        {
            var request = $"UPDATE [items] " +
                          $"SET name=@name, description=@description, " +
                          $"price=@price, category_id=@categoryId" +
                          $"WHERE item_id=@itemId";
            using var connection = new SqlConnection(ConnectionString);
            var result = await connection.ExecuteAsync(request, new
            {
                itemId = item.ItemId,
                name = item.Name,
                description = item.Description,
                price = item.Price,
                categoryId = item.CategoryId
            });
            return result >= 0;
        }

        public async ValueTask<bool> TryDelete(Guid itemId)
        {
            var request = $"DELETE FROM [items]"+
                          $"WHERE item_id=@itemId";
            using var connection = new SqlConnection(ConnectionString);
            var result = await connection.ExecuteAsync(request, new {itemId});
            return result >= 0;
        }

        public async ValueTask<bool> Exists(string name)
        {
            var request = $"SELECT count(*) FROM [items]" +
                          $"WHERE name=@name";
            using var connection = new SqlConnection(ConnectionString);
            var result = await connection.QuerySingleAsync<int>(request, new {name});
            return result > 0;
        }
    }
}