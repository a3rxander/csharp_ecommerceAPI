using ecommerceAPI.src.EcommerceAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ecommerceAPI.src.EcommerceAPI.Persistence.Data
{
    public class EcommerceDbContext : DbContext
    {
        public EcommerceDbContext(DbContextOptions<EcommerceDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
