using System;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Dapper;
using TestShopApplication.Dal.Models;

namespace TestShopApplication.Dal.Repositories
{
    public sealed class AuthRepository : IAuthRepository
    {
        private string ConnectionString { get; }

        public AuthRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public async ValueTask<UserSecurityDetails> GetUserSecurityDetails(string username)
        {
            var query = @"SELECT u.[id] as UserId, u.[password] as PasswordHash, u.[first_name], u.[last_name], r.[role_name] as Role FROM users AS u
                        JOIN user_roles AS r
                        ON u.role_id = r.id
                        WHERE u.username = @username";
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            var dbEntry = await connection.QuerySingleOrDefaultAsync<UserSecurityDetails>(query, new { username });

            if (dbEntry != null)
            {
                dbEntry.UserName = username;
            }
            return dbEntry;
        }
    }
}
