using System;

namespace TestShopApplication.Dal.Models
{
    public class Order
    {
        public string OrderId { get; set; }
        public decimal Price { get; set; }
        public long CreatedTimestamp { get; set; }
        public string Status { get; set; }
    }
}