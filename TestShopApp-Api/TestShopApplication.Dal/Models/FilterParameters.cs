﻿using System;
using System.Collections.Generic;

namespace TestShopApplication.Dal.Models
{
    public readonly struct FilterParameters
    {
        public decimal MinPrice { get; init; }
        public decimal? MaxPrice { get; init; }
        public IList<string> CategoryIds { get; init; }
        public OrderBy? OrderBy { get; init; }

        public FilterParameters(decimal minPrice, decimal? maxPrice, IList<string> categoryList, OrderBy? orderBy)
        {
            MinPrice = minPrice;
            MaxPrice = maxPrice;
            CategoryIds = categoryList;
            OrderBy = orderBy;
        }
    }
}
