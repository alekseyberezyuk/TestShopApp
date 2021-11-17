using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestShopApplication.Dal.Common;

namespace TestShopApplication.Dal.Repositories
{
    public interface IItemsRepository
    {
        Task<IEnumerable<Item>> GetAll(int categoryId);
        Task<Item> GetById(Guid itemId);
        Task<bool> TryAdd(ItemWithCategory item);
        Task<bool> TryUpdate(ItemWithCategory item);
        Task<bool> TryDelete(Guid itemId);
    }
}