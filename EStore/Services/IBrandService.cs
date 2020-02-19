using EStore.Messages.Request.Brand;
using EStore.Messages.Response.Brand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Services
{
    public interface IBrandService
    {
        CreateBrandResponse SaveBrand(CreateBrandRequest brandRequest);
        UpdateBrandResponse EditBrand(UpdateBrandRequest updateBrandRequest);
        GetBrandResponse GetBrand(GetBrandRequest getBrandRequest);
        FetchBrandsResponse GetBrands(FetchBrandRequest fetchBrandsRequest);
        DeleteBrandResponse DeleteBrand(DeleteBrandRequest deleteBrandRequest);
    }
}
