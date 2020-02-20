using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EStore.Messages.Request.Category;
using EStore.Messages.Response.Category;
using EStore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EStore.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
  
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public ActionResult<GetCategoryResponse> GetCategory(long id)
        {
            var getCategoryRequest = new GetCategoryRequest
            {
                Id = id
            };
            var getCategoryResponse = _categoryService.GetCategory(getCategoryRequest);
            return getCategoryResponse;
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult<FetchCategoriesResponse> GetCategories()
        {
            var fetchCategoryRequest = new FetchCategoryRequest { };
            var fetchCategoriesResponse = _categoryService.GetCategories(fetchCategoryRequest);
            return fetchCategoriesResponse;
        }

        //[Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult<CreateCategoryResponse> PostCategory(CreateCategoryRequest createCategoryRequest)
        {
            var createCategoryResponse = _categoryService.SaveCategory(createCategoryRequest);
            return createCategoryResponse;
        }

        //[Authorize(Roles = "Administrator")]
        [HttpPut()]
        public ActionResult<UpdateCategoryResponse> PutCategory(UpdateCategoryRequest updateCategoryRequest)
        {

            var updateCategoryResponse = _categoryService.EditCategory(updateCategoryRequest);

            return updateCategoryResponse;
        }

        //[Authorize(Roles = "Administrator")]
        [HttpDelete("{id}")]
        public ActionResult<DeleteCategoryResponse> DeleteCategory(long id)
        {
            var deleteCategoryRequest = new DeleteCategoryRequest
            {
                Id = id
            };
            var deleteCategoryResponse = _categoryService.DeleteCategory(deleteCategoryRequest);
            return deleteCategoryResponse;
        }
    }
}
