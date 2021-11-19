using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestShopApplication.Dal.Common;

namespace TestShopApplication.Dal.Repositories
{
    public interface IUserCartRepository
    {
        ValueTask<IEnumerable<ShoppingCartContentItem>> GetShoppingCart(Guid userId);
        ValueTask<Guid> AddItemToCart(ShoppingCartItem item);
        ValueTask<bool> RemoveItemFromCart(ShoppingCartItem item);
    }
}