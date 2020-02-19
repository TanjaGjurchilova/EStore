namespace EStore.Messages.Response.Category
{
    using DataTransferObjects.Product;
    public class CreateCategoryResponse:ResponseBase
    {
        public CategoryDto Category { get; set; }
    }
}
