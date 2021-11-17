using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestShopApplication.Dal.Common;
using TestShopApplication.Dal.Repositories;

namespace TestShopApplication.Api.Services
{
    public class ItemsService
    {
        private readonly IItemsRepository _itemsRepository;
        public ItemsService(IItemsRepository itemsRepository)
        {
            _itemsRepository = itemsRepository;
        }
        
        public async Task<IEnumerable<Item>> GetAll(int? categoryId)
        {
            return await _itemsRepository.GetAll(categoryId ?? 0);
        }
        
        public async Task<Item> GetById(Guid itemId)
        {
            return await _itemsRepository.GetById(itemId);
        }
        
        public async Task<bool> Create(ItemWithCategory item)
        {
            return await _itemsRepository.TryAdd(item);
        }
        
        public async Task<bool> Update(ItemWithCategory item)
        {
            return await _itemsRepository.TryUpdate(item);
        }

        public async Task<bool> Delete(Guid itemId)
        {
            return await _itemsRepository.TryDelete(itemId);
        }
    }
}