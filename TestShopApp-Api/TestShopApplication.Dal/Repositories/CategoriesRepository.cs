using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Dapper;
using TestShopApplication.Dal.Models;

namespace TestShopApplication.Dal.Repositories
{
    public sealed class CategoryRepository : ICategoryRepository
    {
        private string ConnectionString { get; }

        public CategoryRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public async ValueTask<UserSecurityDetails> GetUserSecurityDetails(string username)
        {
            var query = @"SELECT u.[id] as UserId, u.[password] as PasswordHash, r.[role_name] as Role FROM users AS u
                        JOIN user_roles AS r
                        ON u.role_id = r.id
                        WHERE u.username = @username";
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            var dbEntry = await connection.QuerySingleAsync<UserSecurityDetails>(query, new { username });
            dbEntry.UserName = username;
            return dbEntry;
        }

        public IEnumerable<Category> GetCategories()
        {
            var query = @"SELECT distinct c.[category_id] AS Id, [category_name] AS Name FROM item_categories AS c
                          INNER JOIN items AS i
                          ON i.category_id=c.category_id";
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            var dbEntry = connection.Query<Category>(query);
            return dbEntry;
        }
    }
}
