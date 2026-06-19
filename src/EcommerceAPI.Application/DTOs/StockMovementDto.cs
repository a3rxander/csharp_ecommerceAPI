namespace ecommerceAPI.src.EcommerceAPI.Application.DTOs
{
    public class StockMovementDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public string MovementType { get; set; } = string.Empty;
        public DateTime MovementDate { get; set; }
        public string Reason { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateStockMovementDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public string MovementType { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
    }

    public class UpdateStockMovementDto
    {
        public int Quantity { get; set; }
        public string MovementType { get; set; } = string.Empty;
        public DateTime MovementDate { get; set; }
        public string Reason { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
