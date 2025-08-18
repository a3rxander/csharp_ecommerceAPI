using System.ComponentModel.DataAnnotations;

namespace ecommerceAPI.src.EcommerceAPI.Domain.Entities
{
    public class Shipping : BaseEntity
    {
        [Required]
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = new Order();
        [Required]
        [MaxLength(200)]
        public string Address { get; set; } = string.Empty;
        public DateTime? ShippedDate { get; set; }
        public DateTime? DeliveredDate { get; set; }
        [MaxLength(50)]
        public string Status { get; set; } = string.Empty;
    }
}
