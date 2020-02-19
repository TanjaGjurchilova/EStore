using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EStore.Messages.Request.Brand;
using EStore.Messages.Response.Brand;
using EStore.Models.Product;
using EStore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EStore.Controllers
{
    [Route("api/[controller]")]
    public class BrandController : Controller
    {
        private readonly IBrandService _brandService;
        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;

        }
        [AllowAnonymous]
        [HttpGet("{id}")]
        public ActionResult<GetBrandResponse> GetBrand(long id)
        {
            var getBrandRequest = new GetBrandRequest
            {
                Id = id
            };
            var getBrandResponse = _brandService.GetBrand(getBrandRequest);
            return getBrandResponse;
        }

        [AllowAnonymous]
        [HttpGet()]
        public ActionResult<FetchBrandsResponse> GetBrands()
        {
            var fetchBrandsRequest = new FetchBrandRequest { };
            var fetchBrandsResponse = _brandService.GetBrands(fetchBrandsRequest);
            return fetchBrandsResponse;
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult<CreateBrandResponse> PostBrand(CreateBrandRequest createBrandRequest) //Model binding
        {
            var createBrandResponse = _brandService.SaveBrand(createBrandRequest);
            return createBrandResponse;
        }

        [Authorize(Roles = "Administrator")]
        [HttpPut()]
        public ActionResult<UpdateBrandResponse> PutBrand(UpdateBrandRequest updateBrandRequest)
        {
            var updateBrandResponse = _brandService.EditBrand(updateBrandRequest);
            return updateBrandResponse;
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id}")]
        public ActionResult<DeleteBrandResponse> DeleteBrand(long id)
        {
            var deleteBrandRequest = new DeleteBrandRequest
            {
                Id = id
            };
            var deleteBrandResponse = _brandService.DeleteBrand(deleteBrandRequest);
            return deleteBrandResponse;
        }
    }
}
