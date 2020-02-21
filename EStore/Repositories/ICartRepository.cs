using EStore.Models.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Repositories
{
    public interface ICartRepository
    {
        Cart FindCartById(long id);
        IEnumerable<Cart> GetAllCarts();
        void SaveCart(Cart cart);
        void UpdateCart(Cart cart);
        void DeleteCart(Cart cart);
    }
}
