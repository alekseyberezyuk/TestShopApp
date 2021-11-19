using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestShopApplication.Dal.Common;

namespace TestShopApplication.Dal.Repositories
{
    public interface IItemsRepository
    {
        ValueTask<IEnumerable<Item>> GetAll(int categoryId);
        ValueTask<Item> GetById(Guid itemId);
        ValueTask<bool> TryAdd(ItemWithCategory item);
        ValueTask<bool> TryUpdate(ItemWithCategory item);
        ValueTask<bool> TryDelete(Guid itemId);
        ValueTask<bool> Exists(string name);
    }
}