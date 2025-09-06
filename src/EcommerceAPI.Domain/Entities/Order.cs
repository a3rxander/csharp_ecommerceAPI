using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; 
namespace ecommerceAPI.src.EcommerceAPI.Domain.Entities
{
    public class Order : BaseEntity
    {
        [Required]
        public  string UserId { get; set;} = string.Empty;
        public  User User { get; set; } = null!;
        
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        public decimal TotalAmount { get; set; }
        [MaxLength(50)]
        public string Status { get; set; } = string.Empty;
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
        public Payment Payment { get; set; } = null!;
        public Shipping Shipping { get; set; } = null!;
    }
}
