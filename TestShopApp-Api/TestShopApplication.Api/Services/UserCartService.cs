using ExtensionsPack.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestShopApplication.Shared.ApiModels;
using TestShopApplication.Dal.Models;
using TestShopApplication.Dal.Repositories;

namespace TestShopApplication.Api.Services
{
    public class UserCartService
    {
        private readonly IUserCartRepository _userCartRepository;
        public UserCartService(IUserCartRepository userCartRepository)
        {
            _userCartRepository = userCartRepository;
        }

        public async Task<IEnumerable<ShoppingCartContentItem>> GetShoppingCart(Guid userId)
        {
            return await _userCartRepository.GetShoppingCart(userId);
        }

        public async Task<Response<Guid>> AddItemToCart(ShoppingCartItem item)
        {
            item.AddedTimeStamp = DateTime.Now.ToUnixUtcTimeStamp();
            var id = await _userCartRepository.AddItemToCart(item);
            return new Response<Guid>
            {
                Success = id != Guid.Empty,
                Result = id
            };
        }

        public async Task<Response<bool>> UpdateItemQuantity(ShoppingCartItem item)
        {
            var existingContent = await _userCartRepository.GetShoppingCartItem(Guid.Parse(item.ItemId),
                Guid.Parse(item.UserId));
            if (existingContent == null) 
            {
                return new Response<bool>
                {
                    Success = false,
                    Errors = new List<string> { $"The item {item.ItemId} doesn't exist in user's {item.UserId} shopping cart" }
                };
            }

            var result = await _userCartRepository.UpdateItemQuantity(item);
            return new Response<bool>
            {
                Success = result != Guid.Empty,
                Result = result != Guid.Empty
            };
        }

        public async Task<Response<bool>> DeleteItemFromCart(ShoppingCartItem item)
        {
            item.AddedTimeStamp = DateTime.Now.ToUnixUtcTimeStamp();
            var result = await _userCartRepository.RemoveItemFromCart(item);
            return new Response<bool>
            {
                Success = result
            };
        }
    }
}