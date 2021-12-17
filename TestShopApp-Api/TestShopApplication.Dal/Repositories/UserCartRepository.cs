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
            var request = $"SELECT [user_carts].item_id, name, description, quantity, " +
                          $"(price * quantity) as price, createdAt " +
                          $"FROM [user_carts] " +
                          $"LEFT JOIN [items] ON [user_carts].item_id = [items].item_id " +
                          $"WHERE user_id = @userId";

            using var connection = new SqliteConnection(ConnectionString);
            var results = await connection.QueryAsync<ShoppingCartContentItem>(request,
                new {userId});
            return results;
        }

        public async ValueTask<Guid> AddItemToCart(ShoppingCartItem item)
        {
            var request = $"IF EXISTS(SELECT * FROM [user_carts] WHERE item_id=@itemId " +
                          $"AND user_id=@userId) " +
                          $"BEGIN " +
                              $"UPDATE [user_carts] " +
                              $"SET quantity=quantity+@quantity, createdAt=@createdAt " +
                              $"WHERE item_id=@itemId AND user_id=@userId " +
                          $"END " +
                          $"ELSE " +
                          $"BEGIN " +
                              $"INSERT INTO [user_carts](item_id, user_id, quantity, createdAt) " +
                              "VALUES (@itemId, @userId, @quantity, @createdAt)" +
                          $"END ";
            await using var connection = new SqliteConnection(ConnectionString);
            var result = await connection.ExecuteAsync(request, new
                {
                    itemId = item.ItemId,
                    userId = item.UserId,
                    quantity = item.Quantity,
                    addedTimeStamp = item.AddedTimeStamp
                });

            return result > 0 ? Guid.Parse(item.ItemId) : Guid.Empty;
        }

        public async ValueTask<bool> RemoveItemFromCart(ShoppingCartItem item)
        {
            var request = $"IF ((select quantity from user_carts " +
                            $"WHERE item_id=@itemId AND user_id=@userId) = @quantity)\r\n" +
                          $"BEGIN\r\n" +
                            $"DELETE FROM user_carts "+
                            $"WHERE item_id=@itemId AND user_id=@userId\r\n"+
                          $"END\r\n" +
                          $"ELSE\r\n" +
                          $"BEGIN\r\n" +
                            $"UPDATE [user_carts] " +
                            $"SET quantity=quantity-1,createdAt=@createdAt " +
                            $"WHERE item_id=@itemId AND user_id=@userId\r\n" +
                          $"END\r\n";
            using var connection = new SqliteConnection(ConnectionString);
            var result = await connection.ExecuteAsync(request, new
            {
                itemId = item.ItemId,
                userId = item.UserId,
                quantity = item.Quantity,
                addedTimeStamp = item.AddedTimeStamp
            });

            return result > 0;
        }
    }
}