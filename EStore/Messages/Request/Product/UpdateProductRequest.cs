namespace EStore.Messages.Request.Product
{
    using DataTransferObjects.Product;
    public class UpdateProductRequest
    {
        public int Id { get; set; }
        public ProductDto Product { get; set; }
    }
}
