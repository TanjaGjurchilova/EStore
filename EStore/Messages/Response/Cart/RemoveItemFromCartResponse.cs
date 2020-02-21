using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Messages.Response.Cart
{
    public class RemoveItemFromCartResponse :ResponseBase
    {
        public long CartItemId { get; set; }
    }
}
