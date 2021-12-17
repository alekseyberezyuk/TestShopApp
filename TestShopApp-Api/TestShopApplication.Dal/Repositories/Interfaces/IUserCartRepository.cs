using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestShopApplication.Dal.Models;

namespace TestShopApplication.Dal.Repositories
{
    public interface IUserCartRepository
    {
        ValueTask<IEnumerable<ShoppingCartContentItem>> GetShoppingCart(Guid userId);
        ValueTask<ShoppingCartItem> GetShoppingCartItem(Guid itemId, Guid userId);
        ValueTask<Guid> AddItemToCart(ShoppingCartItem item);
        ValueTask<Guid> UpdateItemQuantity(ShoppingCartItem item);
        ValueTask<bool> RemoveItemFromCart(ShoppingCartItem item);
    }
}