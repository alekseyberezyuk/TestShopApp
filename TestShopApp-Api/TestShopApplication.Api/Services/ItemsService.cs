using System;
using TestShopApplication.Api.Models;
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
        
        public async Task<IEnumerable<Item>> GetAll(FilterParameters filterParameters)
        {
            return await _itemsRepository.GetAll(filterParameters);
        }
        
        public async Task<Item> GetById(Guid itemId)
        {
            return await _itemsRepository.GetById(itemId);
        }
        
        public async Task<Response> Create(ItemWithCategory item)
        {
            if (await Exists(item.Name))
                return new Response
                {
                    Success = false,
                    Errors = new List<string>
                    {
                        $"Failed to create. Item with name {item.Name} already exists."
                    }
                };

            var addResult = await _itemsRepository.TryAdd(item);

            return new Response
            {
                Success = addResult
            };
        }
        
        public async Task<Response> Update(ItemWithCategory item)
        {
            if (await Exists(item.Name))
                return new Response
                {
                    Success = false,
                    Errors = new List<string>
                    {
                        $"Unable to update.Item with name {item.Name} already exists."
                    }
                };
            
            var updateResult =  await _itemsRepository.TryUpdate(item);

            return new Response
            {
                Success = updateResult
            };
        }

        public async Task<bool> Delete(Guid itemId)
        {
            return await _itemsRepository.TryDelete(itemId);
        }

        private async Task<bool> Exists(string name)
        {
            if (await _itemsRepository.Exists(name))
                return true;

            return false;
        }
    }
}