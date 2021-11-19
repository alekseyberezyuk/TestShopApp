using System;

namespace TestShopApplication.Dal.Common
{
    public class Order
    {
        public Guid OrderId { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public OrderStatus Status { get; set; }
    }
}