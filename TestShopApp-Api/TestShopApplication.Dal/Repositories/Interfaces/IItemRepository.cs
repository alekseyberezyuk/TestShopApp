using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestShopApplication.Dal.Models;

namespace TestShopApplication.Dal.Repositories
{
    public interface IItemRepository
    {
        Task<IEnumerable<Item>> GetAll(FilterParameters filterParameters);
        ValueTask<Item> GetById(Guid itemId);
        ValueTask<Guid> TryAdd(Item item);
        ValueTask<bool> TryUpdate(Item item);
        ValueTask<bool> TryDelete(Guid itemId);
        ValueTask<bool> Exists(string name);
    }
}