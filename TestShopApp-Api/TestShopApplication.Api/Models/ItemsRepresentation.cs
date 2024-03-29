﻿using System;
using System.Collections.Generic;

namespace TestShopApplication.Shared.ApiModels
{
    public sealed class ItemsPresentation
    {
        public int TotalItems { get; set; }
        public IEnumerable<ItemResponsePresentation> Items { get; set; }
    }
}
