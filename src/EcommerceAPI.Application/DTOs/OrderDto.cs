using System.ComponentModel.DataAnnotations;
using ecommerceAPI.src.EcommerceAPI.Domain.Enums;

namespace ecommerceAPI.src.EcommerceAPI.Application.DTOs
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public ShippingDto? Shipping { get; set; }
        public IEnumerable<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
    }

    public class CreateOrderDto
    {
        [Required]
        [MaxLength(200)]
        public string ShippingAddress { get; set; } = string.Empty;
    }

    public class UpdateOrderDto
    {
        public OrderStatus Status { get; set; }
    }
}
