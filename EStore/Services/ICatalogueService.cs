using EStore.Messages.Request.Product;
using EStore.Messages.Response.Product;

namespace EStore.Services
{
    public interface ICatalogueService
    {
        FetchProductsResponse FetchProducts(FetchProductsRequest fetchProductsRequest);
    }
}
