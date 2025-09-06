using System.ComponentModel.DataAnnotations;

namespace ecommerceAPI.src.EcommerceAPI.Domain.Entities
{
    public class OrderItem : BaseEntity
    {
        [Required]
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;
        [Required]
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }
        [Required]
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice => UnitPrice * Quantity;
    }
}
