using System;
using System.Collections.Generic;

namespace TestShopApplication.Api.Models
{
    public sealed class ItemsPresentation
    {
        public int TotalItems { get; set; }
        public IEnumerable<ItemResponsePresentation> Items { get; set; }
    }
}
