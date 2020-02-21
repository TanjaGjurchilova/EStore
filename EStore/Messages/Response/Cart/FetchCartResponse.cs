

namespace EStore.Messages.Response.Cart
{
    using EStore.Messages.DataTransferObjects.Cart;
    public class FetchCartResponse :ResponseBase
    {
        public CartDto Cart { get; set; }
    }
}
