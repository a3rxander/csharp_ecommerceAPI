using System.ComponentModel.DataAnnotations;

namespace ecommerceAPI.src.EcommerceAPI.Domain.Entities
{
    public class Payment : BaseEntity
    {
        [Required]
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;
        [Required]
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        [MaxLength(50)]
        public string Method { get; set; } = string.Empty;
        [MaxLength(50)]
        public string Status { get; set; } = string.Empty;
    }
}
