using EStore.Messages.DataTransferObjects.Product;

namespace EStore.Messages.Response.Brand
{
    public class DeleteBrandResponse: ResponseBase
    {
        public BrandDto Brand { get; set; }
    }
}
