using EStore.Messages.DataTransferObjects.Product;

namespace EStore.Messages.Response.Brand
{
    public class CreateBrandResponse : ResponseBase
    {
        public BrandDto Brand { get; set; }
    }
}
