using System.ComponentModel.DataAnnotations;

namespace ecommerceAPI.src.EcommerceAPI.Domain.Entities
{
    public class Category : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public  string Name { get; set; } = string.Empty;
        [MaxLength(500)]
        public  string Description { get; set; } = string.Empty;

    }
}
