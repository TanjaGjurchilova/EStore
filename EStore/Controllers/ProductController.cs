using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EStore.Messages.Request.Product;
using EStore.Messages.Response.Product;
using EStore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EStore.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public ActionResult<GetProductResponse> GetProduct(long id)
        {
            var getProductRequest = new GetProductRequest
            {
                Id = id
            };
            var getProductResponse = _productService.GetProduct(getProductRequest);
            return getProductResponse;
        }

        [AllowAnonymous]
        [HttpGet("{categorySlug}/{brandSlug}/{page}/{productsPerPage}")]
        public ActionResult<FetchProductsResponse> GetProducts(string categorySlug, string brandSlug, int page, int productsPerPage)
        {
            var fetchProductsRequest = new FetchProductsRequest
            {
                PageNumber = page,
                ProductsPerPage = productsPerPage,
                CategorySlug = categorySlug,
                BrandSlug = brandSlug
            };
            var fetchProductsResponse = _productService.GetProducts(fetchProductsRequest);
            return fetchProductsResponse;
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult<CreateProductResponse> PostProduct(CreateProductRequest createProductRequest)
        {
            var createProductResponse = _productService.SaveProduct(createProductRequest);
            return createProductResponse;
        }

        [Authorize(Roles = "Administrator")]
        [HttpPut()]
        public ActionResult<UpdateProductResponse> PutProduct(UpdateProductRequest updateProductRequest)
        {

            var updateProductResponse = _productService.EditProduct(updateProductRequest);

            return updateProductResponse;
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id}")]
        public ActionResult<DeleteProductResponse> DeleteProduct(long id)
        {
            var deleteProductRequest = new DeleteProductRequest
            {
                Id = id
            };
            var deleteProductResponse = _productService.DeleteProduct(deleteProductRequest);
            return deleteProductResponse;
        }
    }
}
