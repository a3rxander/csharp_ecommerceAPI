using System.ComponentModel.DataAnnotations;

namespace ecommerceAPI.src.EcommerceAPI.Domain.Entities
{
    public class Review : BaseEntity
    {
        [Required]
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = new Product();
        [Required]
        public Guid UserId { get; set; }
        public User User { get; set; } = new User();
        [Range(1, 5)]
        public int Rating { get; set; }
        [MaxLength(1000)]
        public string Comment { get; set; } = string.Empty;
        public DateTime ReviewDate { get; set; }
    }
}
