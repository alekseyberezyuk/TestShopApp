using System;

namespace TestShopApplication.Api.Models
{
    public class ItemResponse
    {
        public Guid ItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        
        public string CategoryName { get; set; }
    }
}