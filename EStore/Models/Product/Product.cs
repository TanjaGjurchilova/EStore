using EStore.Models.Shared;

namespace EStore.Models.Product
{
    public class Product:BaseObject
    {
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public string SKU { get; set; }
        public string Model { get; set; }
        public decimal Price { get; set; }
        public decimal SalePrice { get; set; }
        public decimal OldPrice { get; set; }
        public string ImageUrl { get; set; }
        public int QuantityInStock { get; set; }
        public bool IsBestseller { get; set; }
        public bool IsFeatured { get; set; }
        public long CategoryId { get; set; }
        public Category Category { get; set; }
        public long BrandId { get; set; }
        public Brand Brand { get; set; }
        public ProductStatus ProductStatus { get; set; }
    }
}
