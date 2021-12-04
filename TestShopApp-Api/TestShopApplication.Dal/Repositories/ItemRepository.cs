using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
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
            var sb = new StringBuilder($@"SELECT item_id, name, description, price, i.category_id, ic.category_name, created_timestamp, thumbnail as ThumbnailBase64 FROM [items] i
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
                switch (filterParameters.OrderBy)
                {
                    case OrderBy.CategoryId:
                        sb.Append("ORDER BY i.category_id");
                        break;
                    case OrderBy.CategoryName:
                        sb.Append("ORDER BY i.category_name");
                        break;
                    case OrderBy.PriceAsc:
                        sb.Append("ORDER BY i.price");
                        break;
                    case OrderBy.PriceDesc:
                        sb.Append("ORDER BY i.price DESC");
                        break;
                    case OrderBy.CreatedDate:
                        sb.Append("ORDER BY i.created_timestamp");
                        break;
                    case OrderBy.NameAsc:
                        sb.Append("ORDER BY i.name");
                        break;
                    case OrderBy.NameDesc:
                        sb.Append("ORDER BY i.name DESC");
                        break;
                    case OrderBy.CreatedDateDesc:
                    default:
                        sb.Append("ORDER BY i.created_timestamp DESC");
                        break;
                }
            }
            var query = sb.ToString().TrimEnd(',', ' ');
            using var connection = new SqlConnection(ConnectionString);
            var results = await connection.QueryAsync<Item>(query, parameters);
            return results;
        }

        public async ValueTask<Item> GetById(Guid itemId)
        {
            string query = $"SELECT item_id as ItemId, name, description, price, i.category_id as categoryId, category_name as categoryName FROM [items] i " +
                           $"LEFT JOIN [item_categories] ic ON i.category_id = ic.category_id " +
                           $"WHERE item_id=@itemId AND is_deleted=0";
            using var connection = new SqlConnection(ConnectionString);
            var result = await connection.QuerySingleAsync<Item>(query, new {itemId});
            return result;
        }

        public async ValueTask<Guid> TryAdd(Item item)
        {
            var request = $"INSERT INTO [items] (item_id, name, description, price, category_id, is_deleted)" +
                          $"VALUES(@itemId, @name, @description, @price, @categoryId, 0)";
            using var connection = new SqlConnection(ConnectionString);
            var result = await connection.ExecuteAsync(request, new
            {
                itemId = item.ItemId,
                name = item.Name,
                description = item.Description,
                price = item.Price,
                categoryId = item.CategoryId
            });
            return result > 0 ? item.ItemId : Guid.Empty;
        }

        public async ValueTask<bool> TryUpdate(Item item)
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
            var request = $"UPDATE [items]" +
                          $"SET is_deleted=1" +
                          $"WHERE item_id=@itemId";
            using var connection = new SqlConnection(ConnectionString);
            var result = await connection.ExecuteAsync(request, new { itemId });
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