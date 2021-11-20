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
        
        public async Task<Response<Guid>> Create(ItemModel item)
        {
            if (await Exists(item.Name))
                return new Response<Guid>
                {
                    Success = false,
                    Errors = new List<string>
                    {
                        $"Failed to create. Item with name {item.Name} already exists."
                    }
                };

            var addResult = await _itemsRepository.TryAdd(new ItemWithCategory
            {
                ItemId = Guid.NewGuid(),
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
                CategoryId = item.CategoryId
            });

            return new Response<Guid>
            {
                Result = addResult,
                Success = addResult != Guid.Empty
            };
        }
        
        public async Task<Response<bool>> Update(Guid itemId, ItemModel item)
        {
            if (await Exists(item.Name))
                return new Response<bool>
                {
                    Success = false,
                    Errors = new List<string>
                    {
                        $"Unable to update.Item with name {item.Name} already exists."
                    }
                };
            
            var updateResult =  await _itemsRepository.TryUpdate(new ItemWithCategory
            {
                ItemId = itemId,
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
                CategoryId = item.CategoryId
            });

            return new Response<bool>
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