using EStore.Messages.DataTransferObjects.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Messages.Response.Cart
{
    public class AddItemToCartResponse :ResponseBase
    {
        public CartItemDto CartItem { get; set; }
    }
}
