using System;

namespace TestShopApplication.Dal.Models
{
    public class ShoppingCartItem
    {
        public Guid ItemId { get; set; }
        public Guid UserId { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}