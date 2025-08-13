﻿namespace ecommerceAPI.src.EcommerceAPI.Application.DTOs
{
    public class CategoryDto
    {
        public Guid Id { get;set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class  CreateCategoryDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class UpdateCategoryDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } 
        public bool IsActive { get; set; }
    }
}
