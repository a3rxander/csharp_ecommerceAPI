namespace ecommerceAPI.src.EcommerceAPI.Domain.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; } 
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
