using EStore.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Repositories
{
    public interface IBrandRepository
    {
        Brand FindBrandById(long id);
        IEnumerable<Brand> GetAllBrands();
        void SaveBrand(Brand brand);
        void UpdateBrand(Brand brand);
        void DeleteBrand(Brand brand);
    }
}
