using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestShopApplication.Dal.Common;

namespace TestShopApplication.Dal.Repositories
{
    public interface IItemsRepository
    {
        Task<IEnumerable<Item>> GetAll(FilterParameters filterParameters);
        ValueTask<Item> GetById(Guid itemId);
        ValueTask<Guid> TryAdd(ItemWithCategory item);
        ValueTask<bool> TryUpdate(ItemWithCategory item);
        ValueTask<bool> TryDelete(Guid itemId);
        ValueTask<bool> Exists(string name);
    }
}