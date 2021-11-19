using System;

namespace TestShopApplication.Dal.Common
{
    public class OrderItemDto
    {
        public Guid OrderId { get; set; }
        public Guid ItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}