using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.Sqlite;
using TestShopApplication.Dal.Models;

namespace TestShopApplication.Dal.Repositories
{
    public class UserCartRepository : IUserCartRepository
    {
        private string ConnectionString { get; }

        public UserCartRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }
        
        public async ValueTask<IEnumerable<ShoppingCartContentItem>> GetShoppingCart(Guid userId)
        {
            var request = $"SELECT [user_cart_items].item_id, name, description, quantity, " +
                          $"(price * quantity) as price, added_timestamp " +
                          $"FROM [user_cart_items] " +
                          $"LEFT JOIN [items] ON [user_cart_items].item_id = [items].item_id " +
                          $"WHERE user_id = @userId";

            using var connection = new SqliteConnection(ConnectionString);
            var results = await connection.QueryAsync<ShoppingCartContentItem>(request,
                new {userId = userId.ToString()});
            return results;
        }

        public async ValueTask<ShoppingCartItem> GetShoppingCartItem(Guid itemId, Guid userId)
        {
            var request = $"SELECT item_id, user_id, quantity, added_timestamp " +
                          $"FROM [user_cart_items] " +
                          $"WHERE item_id=@itemId AND user_id=@userId";

            using var connection = new SqliteConnection(ConnectionString);
            var result = await connection.QuerySingleOrDefaultAsync<ShoppingCartContentItem>(request,
                new 
                {
                    itemId = itemId.ToString(),
                    userId = userId.ToString() 
                });

            return result;
        }

        public async ValueTask<Guid> AddItemToCart(ShoppingCartItem item)
        {
            var request = $"INSERT INTO [user_cart_items](item_id, user_id, quantity, added_timestamp) " +
                              "VALUES (@itemId, @userId, @quantity, @addedTimeStamp)";
            await using var connection = new SqliteConnection(ConnectionString);
            var result = await connection.ExecuteAsync(request, new
                {
                    itemId = item.ItemId.ToString(),
                    userId = item.UserId.ToString(),
                    quantity = item.Quantity,
                    addedTimeStamp = item.AddedTimeStamp
                });

            return result > 0 ? Guid.Parse(item.ItemId) : Guid.Empty;
        }

        public async ValueTask<Guid> UpdateItemQuantity(ShoppingCartItem item) 
        {
            var request = $"UPDATE [user_cart_items] " +
                          $"SET quantity=@quantity " +
                          $"WHERE item_id=@itemId AND user_id=@userId ";
            await using var connection = new SqliteConnection(ConnectionString);
            var result = await connection.ExecuteAsync(request, new
            {
                itemId = item.ItemId.ToString(),
                quantity = item.Quantity,
                userId = item.UserId.ToString()
            });

            return Guid.Parse(item.ItemId);
        }

        public async ValueTask<bool> RemoveItemFromCart(ShoppingCartItem item)
        {
            var request = $"DELETE FROM user_cart_items WHERE item_id=@itemId AND user_id=@userId";
            using var connection = new SqliteConnection(ConnectionString);
            var result = await connection.ExecuteAsync(request, new
            {
                itemId = item.ItemId.ToString(),
                userId = item.UserId.ToString(),
            });

            return result > 0;
        }

        
    }
}