using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Messages.DataTransferObjects.Order
{
    public class OrderDto
    {
        public long Id { get; set; }
        public decimal OrderTotal { get; set; }
        public decimal OrderItemTotal { get; set; }
        public decimal ShippingCharge { get; set; }
        public long CustomerId { get; set; }
        public int OrderStatus { get; set; }
        public long AddressId { get; set; }
        public IEnumerable<OrderItemDto> OrderItems { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
