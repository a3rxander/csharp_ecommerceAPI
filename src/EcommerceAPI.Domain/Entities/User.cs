﻿namespace ecommerceAPI.src.EcommerceAPI.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string? FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty; 
        public string Role { get; set; } = "Customer"; // Possible values: "Customer", "Admin", "Seller"    

    }
}
       