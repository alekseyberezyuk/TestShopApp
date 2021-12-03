using System;
using TestShopApplication.Dal.Models;

namespace TestShopApplication.Api.Models
{
    public sealed class CategoryRepresentation
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public CategoryRepresentation(Category c)
        {
            this.Id = c.Id;
            this.Name = c.Name;
        }
    }
}
