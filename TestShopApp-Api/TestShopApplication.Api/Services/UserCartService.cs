using ExtensionsPack.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestShopApplication.Api.Models;
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

        public async Task<Response<bool>> DeleteItemToCart(ShoppingCartItem item)
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