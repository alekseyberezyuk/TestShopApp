using System;

namespace TestShopApplication.Shared.Dtos
{
    public sealed class OrderItemPresentation
    {
        public Guid ItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
    }
}