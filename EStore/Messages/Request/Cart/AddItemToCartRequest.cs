using EStore.Messages.DataTransferObjects.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Messages.Request.Cart
{
    public class AddItemToCartRequest
    {
        public long CartId { get; set; }
        public CartDto CartItem { get; set; }
        public long ProductId { get; set; }
    }
}
