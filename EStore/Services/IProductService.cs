using EStore.Messages.Request.Product;
using EStore.Messages.Response.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Services
{
    public interface IProductService
    {
        CreateProductResponse SaveProduct(CreateProductRequest createProductRequest);
        UpdateProductResponse EditProduct(UpdateProductRequest updateProductRequest);
        GetProductResponse GetProduct(GetProductRequest getProductRequest);
        FetchProductsResponse GetProducts(FetchProductsRequest fetchProductsRequest);
        DeleteProductResponse DeleteProduct(DeleteProductRequest deleteProductRequest);
    }
}
