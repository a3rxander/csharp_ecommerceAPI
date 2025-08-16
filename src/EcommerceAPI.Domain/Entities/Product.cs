using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ecommerceAPI.src.EcommerceAPI.Domain.Entities
{
    public class Product : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        [MinLength(3)]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative.")]
        public int Stock { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        // Navigation properties
        public Category Category { get; set; } = new Category();
        public Guid CategoryId { get; set; } 
    }
}
