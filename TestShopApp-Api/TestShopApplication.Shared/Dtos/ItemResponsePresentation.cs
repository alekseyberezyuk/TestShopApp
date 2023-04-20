using System;

namespace TestShopApplication.Shared.Dtos
{
    public sealed class ItemResponsePresentation
    {
        public Guid ItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public long CreatedTimeStamp { get; set; }
        public string ThumbnailBase64 { get; set; }
    }
}