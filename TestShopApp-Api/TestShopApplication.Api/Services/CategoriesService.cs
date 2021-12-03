using System;
using System.Collections.Generic;
using System.Linq;
using TestShopApplication.Dal.Repositories;
using TestShopApplication.Api.Models;

namespace TestShopApplication.Api.Services
{
    public class CategoriesService
    {
        private ICategoryRepository CategoryRepository { get; }

        public CategoriesService(ICategoryRepository categoryRepository)
        {
            CategoryRepository = categoryRepository;
        }

        public IEnumerable<CategoryRepresentation> GetCategories()
        {
            return CategoryRepository.GetCategories()?.Select(c => new CategoryRepresentation(c));
        }
    }
}
