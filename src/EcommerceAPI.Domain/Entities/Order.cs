using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; 
using ecommerceAPI.src.EcommerceAPI.Domain.Enums;

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
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
        public Payment Payment { get; set; } = null!;
        public Shipping Shipping { get; set; } = null!;
    }
}
