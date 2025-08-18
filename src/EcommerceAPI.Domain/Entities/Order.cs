using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ecommerceAPI.src.EcommerceAPI.Domain.Entities
{
    public class Order : BaseEntity
    {
        [Required]
        public Guid UserId { get; set; }
        public User User { get; set; } = new User();
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        public decimal TotalAmount { get; set; }
        [MaxLength(50)]
        public string Status { get; set; } = string.Empty;
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
        public Payment Payment { get; set; } = new Payment();
        public Shipping Shipping { get; set; } = new Shipping();
    }
}
