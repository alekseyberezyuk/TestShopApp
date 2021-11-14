using System;
using System.Threading.Tasks;
using TestShopApplication.Dal.Common;

namespace TestShopApplication.Dal.Repositories
{
    public interface IAuthRepository
    {
        ValueTask<UserSecurityDetails> GetUserSecurityDetails(string username);
    }
}
