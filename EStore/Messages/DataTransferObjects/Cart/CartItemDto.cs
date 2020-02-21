using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EStore.Messages.DataTransferObjects.Product;
using EStore.Models;

namespace EStore.Messages.DataTransferObjects.Cart
{
    public class CartItemDto
    {
        public long Id { get; set; }
        public long CartId { get; set; }
        public ProductDto Product { get; set; }
        public int Quantity { get; set; }
    }
}
