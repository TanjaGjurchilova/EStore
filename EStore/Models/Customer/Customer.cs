namespace EStore.Models.Customer
{
    using System.Collections.Generic;
    using Shared;
    using Address;
    using Order;
    public class Customer : BaseObject
    {
        public long PersonId { get; set; }
        public Person Person { get; set; }
        public IEnumerable<Order> Orders { get; set; }
        public IEnumerable<Address> Addresses { get; set; }
    }
}
