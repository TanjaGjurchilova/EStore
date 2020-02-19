using EStore.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Repositories
{
    public interface IProductRepository
    {
        Product FindProductById(long id);
        IEnumerable<Product> GetAllProducts();
        void SaveProduct(Product Product);
        void UpdateProduct(Product Product);
        void DeleteProduct(Product Product);
    }
}
