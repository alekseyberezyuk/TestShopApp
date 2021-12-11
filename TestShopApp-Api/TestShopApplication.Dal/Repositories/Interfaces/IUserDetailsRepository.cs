using System;
using TestShopApplication.Dal.Models;

namespace TestShopApplication.Dal.Repositories
{
    public interface IUserDetailsRepository
    {
        UserDetails GetUserDetails(Guid userId);
        bool UpdateUserDetails(UserDetails userDetails);
    }
}
