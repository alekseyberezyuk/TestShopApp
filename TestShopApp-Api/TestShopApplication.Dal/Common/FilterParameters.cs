using System;
using System.Collections.Generic;

namespace TestShopApplication.Dal.Common
{
    public readonly struct FilterParameters
    {
        public decimal MinPrice { get; init; }
        public decimal? MaxPrice { get; init; }
        public IList<string> CategoryIds { get; init; }

        public FilterParameters(decimal minPrice, decimal? maxPrice, IList<string> categoryList)
        {
            MinPrice = minPrice;
            MaxPrice = maxPrice;
            CategoryIds = categoryList;
        }
    }
}
