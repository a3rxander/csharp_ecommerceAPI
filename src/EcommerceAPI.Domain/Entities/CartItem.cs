using System.ComponentModel.DataAnnotations;

namespace ecommerceAPI.src.EcommerceAPI.Domain.Entities
{
    public class CartItem : BaseEntity
    {
        [Required]
        public Guid CartId { get; set; }
        public Cart Cart { get; set; } = null!;

        [Required]
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }
    }
}
