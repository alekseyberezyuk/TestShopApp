using System;

namespace TestShopApplication.Dal.Models
{
    public sealed class UserSecurityDetails
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public ShopAppUserRole Role { get; set; }
    }
}
