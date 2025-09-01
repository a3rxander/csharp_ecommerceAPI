using System.ComponentModel.DataAnnotations;

namespace ecommerceAPI.src.EcommerceAPI.Domain.Entities
{

    public class ProductImage : BaseEntity
    {
        [Required]
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = new Product();
        [Required]
        [MaxLength(255)]
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsPrimary { get; set; }
    }

}