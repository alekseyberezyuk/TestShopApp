using System;

namespace TestShopApplication.Dal.Models
{
    public class ShoppingCartItem
    {
        public string ItemId { get; set; }
        public string UserId { get; set; }
        public int Quantity { get; set; }
        public long AddedTimeStamp { get; set; }
    }
}