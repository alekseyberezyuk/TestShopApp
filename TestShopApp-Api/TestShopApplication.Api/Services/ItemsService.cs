using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestShopApplication.Dal.Models;
using TestShopApplication.Dal.Repositories;
using TestShopApplication.Api.Models;

namespace TestShopApplication.Api.Services
{
    public class ItemsService
    {
        private readonly IItemRepository _itemsRepository;
        public ItemsService(IItemRepository itemsRepository)
        {
            _itemsRepository = itemsRepository;
        }
        
        public async Task<IEnumerable<ItemResponsePresentation>> GetAll(FilterParameters filterParameters)
        {
            IEnumerable<Item> items = await _itemsRepository.GetAll(filterParameters);
            return items.Select(i => new ItemResponsePresentation(i));
        }
        
        public async Task<ItemResponsePresentation> GetById(Guid itemId)
        {
            return new ItemResponsePresentation(await _itemsRepository.GetById(itemId));
        }
        
        public async Task<Response<Guid>> Create(ItemPresentation item)
        {
            if (await Exists(item.Name))
            {
                return new Response<Guid>
                {
                    Success = false,
                    Errors = new List<string>
                    {
                        $"Failed to create. Item with name {item.Name} already exists."
                    }
                };
            }
            var addResult = await _itemsRepository.TryAdd(new Item
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
        
        public async Task<Response<bool>> Update(Guid itemId, ItemPresentation item)
        {
            if (await Exists(item.Name))
            {
                return new Response<bool>
                {
                    Success = false,
                    Errors = new List<string>
                    {
                        $"Unable to update.Item with name {item.Name} already exists."
                    }
                };
            }
            var updateResult =  await _itemsRepository.TryUpdate(new Item
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
            return await _itemsRepository.Exists(name);
        }
    }
}