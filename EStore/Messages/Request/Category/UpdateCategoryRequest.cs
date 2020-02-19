namespace EStore.Messages.Request.Category
{
    using DataTransferObjects.Product;
    public class UpdateCategoryRequest
    {
        public int Id { get; set; }
        public CategoryDto Category { get; set; }
    }
}
