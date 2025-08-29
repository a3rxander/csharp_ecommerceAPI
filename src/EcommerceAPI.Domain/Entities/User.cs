using Microsoft.AspNetCore.Identity;

namespace ecommerceAPI.src.EcommerceAPI.Domain.Entities
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime CreatedAt { get; set; } 
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsActive { get; set; }
        public string Role { get; set; } = "Customer";   

    }
}
       