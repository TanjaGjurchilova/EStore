namespace EStore.Messages.Response.Category
{
    using DataTransferObjects.Product;
    public class DeleteCategoryResponse:ResponseBase
    {
        public CategoryDto Category { get; set; }
    }
}
