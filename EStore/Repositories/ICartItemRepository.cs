using EStore.Models.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Repositories
{
    public interface ICartItemRepository
    {
        CartItem FindCartItemById(long id);
        IEnumerable<CartItem> FindCartItemsByCartId(long cartId);
        void SaveCartItem(CartItem cartItem);
        void UpdateCartItem(CartItem cartItem);
        void DeleteCartItem(CartItem cartItem);
    }
}
