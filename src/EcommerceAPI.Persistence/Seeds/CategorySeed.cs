using ecommerceAPI.src.EcommerceAPI.Domain.Entities;
using ecommerceAPI.src.EcommerceAPI.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ecommerceAPI.src.EcommerceAPI.Persistence.Seeds
{
    public static class CategorySeed  
    {
        public static void SeedCategories(EcommerceDbContext context)
        {
            if(!context.Categories.Any())
            {
                context.Categories.AddRange(
                    new Category
                    {
                        Id = new Guid("d290f1ee-6c54-4b01-90e6-d701748f0851"),
                        Name = "Electronics",
                        Description = "Devices and gadgets",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        IsActive = true
                    },
                    new Category
                    {
                        Id = new Guid("c4a760a4-5b63-4d3b-9c1a-2f8f3e6f7e6b"),
                        Name = "Books",
                        Description = "Literature and educational materials",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        IsActive = true
                    },
                    new Category
                    {
                        Id = new Guid("f7c3b1d8-3e2a-4f5b-9c6d-1e2f3a4b5c6d"),
                        Name = "Clothing",
                        Description = "Apparel and fashion items",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        IsActive = true
                    }
                );
                context.SaveChanges();
            }
        }

    }
}
