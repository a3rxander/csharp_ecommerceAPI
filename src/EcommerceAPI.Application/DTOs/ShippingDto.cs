namespace ecommerceAPI.src.EcommerceAPI.Application.DTOs
{
    public class ShippingDto
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public string Address { get; set; } = string.Empty;
        public DateTime? ShippedDate { get; set; }
        public DateTime? DeliveredDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateShippingDto
    {
        public Guid OrderId { get; set; }
        public string Address { get; set; } = string.Empty;
        public DateTime? ShippedDate { get; set; }
        public DateTime? DeliveredDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class UpdateShippingDto
    {
        public string Address { get; set; } = string.Empty;
        public DateTime? ShippedDate { get; set; }
        public DateTime? DeliveredDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
