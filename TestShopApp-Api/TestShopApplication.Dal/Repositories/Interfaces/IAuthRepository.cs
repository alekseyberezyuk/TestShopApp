using System;
using System.Threading.Tasks;
using TestShopApplication.Dal.Models;

namespace TestShopApplication.Dal.Repositories
{
    public interface IAuthRepository
    {
        ValueTask<UserSecurityDetails> GetUserSecurityDetails(string username);
    }
}
