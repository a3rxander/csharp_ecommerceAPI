namespace ecommerceAPI.src.EcommerceAPI.Application.DTOs
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public IEnumerable<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
    }

    public class CreateOrderDto
    {
        public IEnumerable<CreateOrderItemDto> Items { get; set; } = new List<CreateOrderItemDto>();
    }

    public class UpdateOrderDto
    {
        public string Status { get; set; } = string.Empty;
    }
}
