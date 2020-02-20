namespace EStore.Models.Cart
{
    using Shared;
    using Product;

    public class CartItem : BaseObject
    {
        public long CartId { get; set; }
        public Cart Cart { get; set; }
        public long ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
