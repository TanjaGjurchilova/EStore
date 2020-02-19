using EStore.Messages;
using EStore.Messages.Request.Category;
using EStore.Messages.Response.Category;
using EStore.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private MessageMapper _messageMapper;
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
            _messageMapper = new MessageMapper();
        }

        public CreateCategoryResponse SaveCategory(CreateCategoryRequest createCategoryRequest)
        {
            var category = _messageMapper.MapToCategory(createCategoryRequest.Category);
            _categoryRepository.SaveCategory(category);
            var categoryDto = _messageMapper.MapToCategoryDto(category);

            var createCategoryResponse = new CreateCategoryResponse
            {
                Category = categoryDto
            };

            return createCategoryResponse;
        }

        public UpdateCategoryResponse EditCategory(UpdateCategoryRequest updateCategoryRequest)
        {
            UpdateCategoryResponse updateCategoryResponse = null;

            if (updateCategoryRequest.Id == updateCategoryRequest.Category.Id)
            {
                var category = _messageMapper.MapToCategory(updateCategoryRequest.Category);
                _categoryRepository.UpdateCategory(category);
                var categoryDto = _messageMapper.MapToCategoryDto(category);

                updateCategoryResponse = new UpdateCategoryResponse
                {

                };
            }
            return updateCategoryResponse;
        }

        public GetCategoryResponse GetCategory(GetCategoryRequest getCategoryRequest)
        {
            GetCategoryResponse getCategoryResponse = null;

            if (getCategoryRequest.Id > 0)
            {
                var category = _categoryRepository.FindCategoryById(getCategoryRequest.Id);
                var categoryDto = _messageMapper.MapToCategoryDto(category);

                getCategoryResponse = new GetCategoryResponse
                {
                    Category = categoryDto
                };
            }

            return getCategoryResponse;
        }

        public FetchCategoriesResponse GetCategories(FetchCategoryRequest fetchCategoryRequest)
        {
            var categories = _categoryRepository.GetAllCategories();
            var categoryDtos = _messageMapper.MapToCategoryDtos(categories);

            return new FetchCategoriesResponse
            {
                Categories = categoryDtos
            };
        }

        public DeleteCategoryResponse DeleteCategory(DeleteCategoryRequest deleteCategoryRequest)
        {
            var category = _categoryRepository.FindCategoryById(deleteCategoryRequest.Id);
            _categoryRepository.DeleteCategory(category);
            var categoryDto = _messageMapper.MapToCategoryDto(category);

            return new DeleteCategoryResponse
            {
                Category = categoryDto
            };
        }
    }
}
