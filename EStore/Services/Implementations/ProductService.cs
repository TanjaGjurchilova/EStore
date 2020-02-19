using EStore.Messages;
using EStore.Messages.Request.Product;
using EStore.Messages.Response.Product;
using EStore.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICatalogueService _catalogueService;
        private MessageMapper _messageMapper;
        public ProductService(IProductRepository productRepository, ICatalogueService catalogueService)
        {
            _productRepository = productRepository;
            _catalogueService = catalogueService;
            _messageMapper = new MessageMapper();
        }

        public CreateProductResponse SaveProduct(CreateProductRequest createProductRequest)
        {
            var product = _messageMapper.MapToProduct(createProductRequest.Product);
            _productRepository.SaveProduct(product);
            var productDto = _messageMapper.MapToProductDto(product);

            var createProductResponse = new CreateProductResponse
            {
                Product = productDto
            };

            return createProductResponse;
        }

        public UpdateProductResponse EditProduct(UpdateProductRequest updateProductRequest)
        {
            UpdateProductResponse updateProductResponse = null;

            if (updateProductRequest.Id == updateProductRequest.Product.Id)
            {
                var product = _messageMapper.MapToProduct(updateProductRequest.Product);
                _productRepository.UpdateProduct(product);
                var productDto = _messageMapper.MapToProductDto(product);

                updateProductResponse = new UpdateProductResponse
                {

                };
            }
            return updateProductResponse;
        }

        public GetProductResponse GetProduct(GetProductRequest getProductRequest)
        {
            GetProductResponse getProductResponse = null;

            if (getProductRequest.Id > 0)
            {
                var product = _productRepository.FindProductById(getProductRequest.Id);
                var productDto = _messageMapper.MapToProductDto(product);

                getProductResponse = new GetProductResponse
                {
                    Product = productDto
                };
            }

            return getProductResponse;
        }

        public FetchProductsResponse GetProducts(FetchProductsRequest fetchProductsRequest)
        {
            var fetchProductsResponse = _catalogueService.FetchProducts(fetchProductsRequest);
            return fetchProductsResponse;
        }

        public DeleteProductResponse DeleteProduct(DeleteProductRequest deleteProductRequest)
        {
            var product = _productRepository.FindProductById(deleteProductRequest.Id);
            _productRepository.DeleteProduct(product);
            var productDto = _messageMapper.MapToProductDto(product);

            return new DeleteProductResponse
            {
                Product = productDto
            };
        }
    }
}
