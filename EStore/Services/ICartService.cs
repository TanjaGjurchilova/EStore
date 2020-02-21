using EStore.Messages.Request.Cart;
using EStore.Messages.Response.Cart;
using EStore.Models.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Services
{
    public interface ICartService
    {
        AddItemToCartResponse AddItemToCart(AddItemToCartRequest addItemToCartRequest);
        RemoveItemFromCartResponse RemoveItemFromCart(RemoveItemFromCartRequest removeItemFromCartRequest);
        string UniqueCartId();
        Cart GetCart();
        FetchCartResponse FetchCart();
        IEnumerable<CartItem> GetCartItems();
        int CartItemsCount();
        decimal GetCartTotal();
    }
}
