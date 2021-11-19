using System.Reflection.Metadata;

namespace TestShopApplication.Dal.Common
{
    public class ShoppingCartContentItem : ShoppingCartItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}