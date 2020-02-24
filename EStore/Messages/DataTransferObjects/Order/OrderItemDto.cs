using EStore.Messages.DataTransferObjects.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Messages.DataTransferObjects.Order
{
    public class OrderItemDto
    {
        public long Id { get; set; }
        public long OrderId { get; set; }
        public long ProductId { get; set; }
        public ProductDto Product { get; set; }
        public int Quantity { get; set; }
    }
}
