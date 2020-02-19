using EStore.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Repositories
{
    public interface ICategoryRepository
    {
        Category FindCategoryById(long id);
        IEnumerable<Category> GetAllCategories();
        void SaveCategory(Category Category);
        void UpdateCategory(Category Category);
        void DeleteCategory(Category Category);
    }
}
