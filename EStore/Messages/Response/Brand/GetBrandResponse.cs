using EStore.Messages.DataTransferObjects.Product;

namespace EStore.Messages.Response.Brand
{
    public class GetBrandResponse : ResponseBase
    {
        public BrandDto Brand { get; set; }
    }
}
