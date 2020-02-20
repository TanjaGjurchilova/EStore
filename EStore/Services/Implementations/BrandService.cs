using EStore.Messages;
using EStore.Messages.Request.Brand;
using EStore.Messages.Response.Brand;
using EStore.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace EStore.Services.Implementations
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepository;
        private MessageMapper _messageMapper;
        public BrandService(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
            _messageMapper = new MessageMapper();
        }
        public DeleteBrandResponse DeleteBrand(DeleteBrandRequest deleteBrandRequest)
        {
            var brand = _brandRepository.FindBrandById(deleteBrandRequest.Id);
            _brandRepository.DeleteBrand(brand);
            var brandDto = _messageMapper.MapToBrandDto(brand);

            return new DeleteBrandResponse
            {
                Brand = brandDto
            };
        }

        public UpdateBrandResponse EditBrand(UpdateBrandRequest updateBrandRequest)
        {
           UpdateBrandResponse updateBrandResponse = null;

            if (updateBrandRequest.Id == updateBrandRequest.Brand.Id)
            {
                var brand = _messageMapper.MapToBrand(updateBrandRequest.Brand);
                _brandRepository.UpdateBrand(brand);
                var brandDto = _messageMapper.MapToBrandDto(brand);

                updateBrandResponse = new UpdateBrandResponse
                {

                };
            }
            return updateBrandResponse;
        }

        public GetBrandResponse GetBrand(GetBrandRequest getBrandRequest)
        {
            GetBrandResponse getBrandResponse = null;

            if (getBrandRequest.Id > 0)
            {
                var brand = _brandRepository.FindBrandById(getBrandRequest.Id);
                var brandDto = _messageMapper.MapToBrandDto(brand);

                getBrandResponse = new GetBrandResponse
                {
                    Brand = brandDto
                };
            }

            return getBrandResponse;
        }

        public FetchBrandsResponse GetBrands(FetchBrandRequest fetchBrandsRequest)
        {
            var brands = _brandRepository.GetAllBrands();
            var brandDtos = _messageMapper.MapToBrandDtos(brands);

            return new FetchBrandsResponse
            {
                Brands = brandDtos
            };
        }

        public CreateBrandResponse SaveBrand(CreateBrandRequest brandRequest)
        {
            var createBrandResponse = new CreateBrandResponse();

            var brand = _messageMapper.MapToBrand(brandRequest.Brand);
            try
            {
                _brandRepository.SaveBrand(brand);
                var brandDto = _messageMapper.MapToBrandDto(brand);
                createBrandResponse.Brand = brandDto;
                createBrandResponse.Messages.Add("Successfully saved the brand");
                createBrandResponse.StatusCode = HttpStatusCode.Created;

            }
            catch (Exception e)
            {
                var error = e.ToString();
                createBrandResponse.Messages.Add(error);
                createBrandResponse.StatusCode = HttpStatusCode.InternalServerError;
            }

            return createBrandResponse;
        }
    }
}
