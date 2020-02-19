namespace EStore.Messages.Response.Product
{
    using DataTransferObjects.Product;
    public class GetProductResponse: ResponseBase
    {
        public ProductDto Product { get; set; }
    }
}
