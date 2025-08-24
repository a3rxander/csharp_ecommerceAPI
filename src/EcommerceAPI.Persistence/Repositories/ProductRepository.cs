using ecommerceAPI.src.EcommerceAPI.Domain.Entities;
using ecommerceAPI.src.EcommerceAPI.Domain.Repositories;
using ecommerceAPI.src.EcommerceAPI.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace ecommerceAPI.src.EcommerceAPI.Persistence.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly EcommerceDbContext _db;

        public ProductRepository(EcommerceDbContext context)
        {
            _db = context;
        }

        public async Task<Product> AddAsync(Product product)
        {
            await _db.Products.AddAsync(product);
            await _db.SaveChangesAsync();
            return product;
        }

        public async Task DeleteAsync(Guid id)
        {
            await _db.Products
                .Where(p => p.Id == id)
                .ExecuteDeleteAsync();
        }

        public Task<bool> ExistsAsync(Guid id)
        {
            return _db.Products
                .AnyAsync(p => p.Id == id && p.IsActive);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _db.Products
                .Where(p => p.IsActive)
                .Include(p => p.Category) // Eager loading of Category
                .Include(p => p.Seller) // Eager loading of Seller
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            return await _db.Products
                .Where(p => p.Id == id && p.IsActive)
                .Include(p => p.Category) // Eager loading of Category
                .Include(p => p.Seller) // Eager loading of Seller
                .FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            product.UpdatedAt = DateTime.UtcNow;
            _db.Products.Update(product);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(Guid categoryId)
        {
            return await _db.Products
                .Where(p => p.CategoryId == categoryId && p.IsActive)
                .Include(p => p.Seller) // Eager loading of Seller
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsBySellerAsync(Guid sellerId)
        {
            return await _db.Products
                .Where(p => p.SellerId == sellerId && p.IsActive)
                .Include(p => p.Category) // Eager loading of Category
                .OrderBy(p => p.Name)
                .ToListAsync();
        }
    }
}
