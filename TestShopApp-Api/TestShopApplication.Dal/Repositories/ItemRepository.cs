using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Dapper;
using TestShopApplication.Dal.Models;

namespace TestShopApplication.Dal.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private string ConnectionString { get; }

        public ItemRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }
        
        public async Task<IEnumerable<Item>> GetAll(FilterParameters filterParameters)
        {
            var withCategories = filterParameters.CategoryIds?.Count > 0;
            var withFilter = filterParameters.MinPrice > 0 || filterParameters.MaxPrice != null || withCategories;
            var thumbnailsSection = filterParameters.IncludeThumbnails ? ", thumbnail as ThumbnailBase64" : "";
            var sb = new StringBuilder($@"SELECT item_id, name, description, price, i.category_id, ic.category_name, created_timestamp as CreatedTimeStamp{thumbnailsSection} FROM [items] i
                                          LEFT JOIN [item_categories] ic
                                            ON i.category_id=ic.category_id 
                                            WHERE is_deleted=0");
            var parameters = new DynamicParameters();

            if (withFilter)
            {
                if (filterParameters.MinPrice > 0)
                {
                    sb.Append(" AND i.price >= @minPrice ");
                    parameters.Add("minPrice", filterParameters.MinPrice);
                }
                if (filterParameters.MaxPrice != null)
                {
                    sb.Append($" AND i.price <= @maxPrice ");
                    parameters.Add("maxPrice", filterParameters.MaxPrice);
                }
                if (withCategories)
                {
                    sb.Append(" AND i.category_id IN (");

                    for (var i = 1; i <= filterParameters.CategoryIds.Count; i++)
                    {
                        var delimiter = i < filterParameters.CategoryIds.Count ? ", " : "";
                        sb.Append($"@categoryId{i}{delimiter}");
                        parameters.Add($"categoryId{i}", filterParameters.CategoryIds[i - 1]);
                    }
                    sb.Append(") ");
                }
            }
            var query = sb.ToString().TrimEnd(',', ' ');
            using var connection = new SqliteConnection(ConnectionString);
            var results = await connection.QueryAsync<Item>(query, parameters);
            return results;
        }

        public async ValueTask<Item> GetById(Guid itemId)
        {
            string query = $"SELECT item_id as ItemId, name, description, price, i.category_id as categoryId, category_name as categoryName FROM [items] i " +
                           $"LEFT JOIN [item_categories] ic ON i.category_id = ic.category_id " +
                           $"WHERE item_id=@itemId AND is_deleted=0";
            using var connection = new SqliteConnection(ConnectionString);
            var result = await connection.QuerySingleAsync<Item>(query, new
            {
                itemId = itemId.ToString()
            });
            return result;
        }

        public async ValueTask<Guid> TryAdd(Item item)
        {
            var request = $"INSERT INTO [items] (item_id, name, description, price, category_id, is_deleted) " +
                          $"VALUES (@itemId, @name, @description, @price, @categoryId, 0)";
            using var connection = new SqliteConnection(ConnectionString);
            var result = await connection.ExecuteAsync(request, new
            {
                itemId = item.ItemId.ToString(),
                name = item.Name,
                description = item.Description,
                price = item.Price,
                categoryId = item.CategoryId
            });
            return result > 0 ? Guid.Parse(item.ItemId) : Guid.Empty;
        }

        public async ValueTask<bool> TryUpdate(Item item)
        {
            var request = $"UPDATE [items] " +
                          $"SET name=@name, description=@description, " +
                          $"price=@price, category_id=@categoryId " +
                          $"WHERE item_id=@itemId";
            using var connection = new SqliteConnection(ConnectionString);
            var result = await connection.ExecuteAsync(request, new
            {
                itemId = item.ItemId.ToString(),
                name = item.Name,
                description = item.Description,
                price = item.Price,
                categoryId = item.CategoryId
            });
            return result >= 0;
        }

        public async ValueTask<bool> TryDelete(Guid itemId)
        {
            var request = $"UPDATE [items] " +
                          $"SET is_deleted=1 " +
                          $"WHERE item_id=@itemId";
            using var connection = new SqliteConnection(ConnectionString);
            var result = await connection.ExecuteAsync(request, new { itemId = itemId.ToString()});
            return result >= 0;
        }

        public async ValueTask<bool> Exists(string name)
        {
            var request = $"SELECT count(*) FROM [items] " +
                          $"WHERE name=@name";
            using var connection = new SqliteConnection(ConnectionString);
            var result = await connection.QuerySingleAsync<int>(request, new {name});
            return result > 0;
        }
    }
}