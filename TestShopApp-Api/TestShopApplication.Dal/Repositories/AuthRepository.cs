using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Dapper;
using TestShopApplication.Dal.Common;

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
            var query = @"SELECT u.[id] as UserId, u.[password] as PasswordHash, r.[role_name] as Role FROM users AS u
                        JOIN user_roles AS r
                        ON u.role_id = r.id
                        WHERE u.username = @username";
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();
            var dbEntry = await connection.QuerySingleAsync<UserSecurityDetails>(query, new { username });
            dbEntry.UserName = username;
            return dbEntry;
        }
    }
}
