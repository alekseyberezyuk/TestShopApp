using System;

namespace TestShopApplication.Dal.Models
{
    public class OrderItemDto
    {
        public string OrderId { get; set; }
        public string ItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}