namespace TestShopApplication.Api.Models
{
    public sealed class ItemPresentation
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int CategoryId { get; set; }
    }
}