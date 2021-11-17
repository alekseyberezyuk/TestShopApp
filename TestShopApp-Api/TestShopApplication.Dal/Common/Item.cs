using System;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace TestShopApplication.Dal.Common
{
    public class Item
    {
        public Guid ItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public string CategoryName { get; set; }
    }
}