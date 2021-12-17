using System;

namespace TestShopApplication.Dal.Models
{
    public sealed class Item
    {
        public string ItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public long CreatedTimeStamp { get; set; }
        public string ThumbnailBase64 { get; set; }
    }
}