using ecommerceAPI.src.EcommerceAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ecommerceAPI.src.EcommerceAPI.Persistence.Seeds
{
    public class CategorySeed : IEntityTypeConfiguration<Category>
    {

        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasData(
                new Category
                {
                    Id = Guid.NewGuid(),
                    Name = "Electronics",
                    Description = "Devices and gadgets"
                },
                new Category
                {
                    Id = Guid.NewGuid(),
                    Name = "Books",
                    Description = "Literature and educational materials"
                },
                new Category
                {
                    Id = Guid.NewGuid(),
                    Name = "Clothing",
                    Description = "Apparel and fashion items"
                }
            );
        }

    }
}
