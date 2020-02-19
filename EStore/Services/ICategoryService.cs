using EStore.Messages.Request.Category;
using EStore.Messages.Response.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Services
{
    public interface ICategoryService
    {
        CreateCategoryResponse SaveCategory(CreateCategoryRequest createCategoryRequest);
        UpdateCategoryResponse EditCategory(UpdateCategoryRequest updateCategoryRequest);
        GetCategoryResponse GetCategory(GetCategoryRequest getCategoryRequest);
        FetchCategoriesResponse GetCategories(FetchCategoryRequest fetchCategoryRequest);
        DeleteCategoryResponse DeleteCategory(DeleteCategoryRequest deleteCategoryRequest);
    }
}
