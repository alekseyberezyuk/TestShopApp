using System;
using System.Collections.Generic;
using TestShopApplication.Dal.Models;

namespace TestShopApplication.Dal.Repositories
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetCategories();
    }
}