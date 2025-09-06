using System.ComponentModel.DataAnnotations;

namespace ecommerceAPI.src.EcommerceAPI.Domain.Entities
{
    public class StockMovement : BaseEntity
    {
        [Required]
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be positive.")]
        public int Quantity { get; set; }
        [MaxLength(50)]
        public string MovementType { get; set; } = string.Empty;
        public DateTime MovementDate { get; set; }
        [MaxLength(200)]
        public string Reason { get; set; } = string.Empty;
    }
}
