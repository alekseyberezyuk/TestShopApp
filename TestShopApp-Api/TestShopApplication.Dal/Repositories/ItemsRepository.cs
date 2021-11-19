using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        
        public async Task<IEnumerable<Item>> GetAll(FilterParameters filterParameters)
        {
            var withCategories = filterParameters.CategoryIds.Any();
            var withFilter = filterParameters.MinPrice > 0 || filterParameters.MaxPrice != null || withCategories;
            var sb = new StringBuilder($@"SELECT item_id, name, description, price, [items].category_id, category_name FROM [items]
                                          LEFT JOIN [item_categories]
                                            ON [items].category_id=[item_categories].category_id ");
            var parameters = new DynamicParameters();

            if (withFilter)
            {
                bool parameterAdded = false;
                sb.Append("WHERE ");

                if (filterParameters.MinPrice > 0)
                {
                    sb.Append("[items].price > @minPrice ");
                    parameters.Add("minPrice", filterParameters.MinPrice);
                    parameterAdded = true;
                }
                if (filterParameters.MaxPrice != null)
                {
                    if (parameterAdded)
                    {
                        sb.Append("AND ");
                    }
                    sb.Append($"[items].price < @maxPrice ");
                    parameters.Add("maxPrice", filterParameters.MaxPrice);
                    parameterAdded = true;
                }

                if (filterParameters.CategoryIds?.Count > 0)
                {
                    if (parameterAdded)
                    {
                        sb.Append("AND ");
                    }
                    sb.Append("[items].category_id IN (");

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
            using var connection = new SqlConnection(ConnectionString);
            var results = await connection.QueryAsync<Item>(query, parameters);
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