using System;
using TestShopApplication.Dal.Models;

namespace TestShopApplication.Api.Models
{
    public sealed class ItemResponsePresentation
    {
        public Guid ItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public long CreatedTimeStamp { get; set; }
        public string ThumbnailBase64 { get; set; }

        public ItemResponsePresentation(Item item)
        {
            ItemId = item.ItemId;
            Name = item.Name;
            Description = item.Description;
            Price = item.Price;
            CategoryId = item.CategoryId;
            CategoryName = item.CategoryName;
            CreatedTimeStamp = item.CreatedTimeStamp;
            ThumbnailBase64 = item.ThumbnailBase64;
        }
    }
}