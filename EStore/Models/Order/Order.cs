namespace EStore.Models.Order
{
    using System.Collections.Generic;
    using Shared;
    using Customer;
    using Address;
    public class Order : BaseObject
    {
        public decimal OrderTotal { get; set; }
        public decimal OrderItemTotal { get; set; }
        public decimal ShippingCharge { get; set; }
        public long CustomerId { get; set; }
        public Customer Customer { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public long AddressId { get; set; }
        public Address DeliveryAddress { get; set; }
        public IEnumerable<OrderItems> OrderItems { get; set; }
    }
}
