namespace ecommerceAPI.src.EcommerceAPI.Application.DTOs
{
    public class ProductImageDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsPrimary { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateProductImageDto
    {
        public Guid ProductId { get; set; }
        public IFormFile ImageFile { get; set; } = null!;
        public bool IsPrimary { get; set; }
    }

    public class UpdateProductImageDto
    {
        public bool IsPrimary { get; set; }
    }
}