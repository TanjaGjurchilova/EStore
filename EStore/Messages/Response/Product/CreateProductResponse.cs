namespace EStore.Messages.Response.Product
{
    using DataTransferObjects.Product;
    public class CreateProductResponse :ResponseBase
    {
        public ProductDto Product { get; set; }
    }
}
