namespace EStore.Models.Order
{
    using Shared;
    using Product;
    public class OrderItems: BaseObject
    {
        public long OrderId { get; set; }
        public Order Order { get; set; }
        public long ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
