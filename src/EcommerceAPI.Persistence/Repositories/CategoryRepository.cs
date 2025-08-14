

using ecommerceAPI.src.EcommerceAPI.Domain.Entities;
using ecommerceAPI.src.EcommerceAPI.Domain.Repositories;
using ecommerceAPI.src.EcommerceAPI.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace ecommerceAPI.src.EcommerceAPI.Persistence.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly EcommerceDbContext _db;

        public CategoryRepository( EcommerceDbContext context)
        {
            _db = context;
        }
        public async Task<Category> AddAsync(Category category)
        {
            await _db.Categories.AddAsync(category);
            await _db.SaveChangesAsync();
            return category;
        }

        public async Task DeleteAsync(Guid id)
        {
            await _db.Categories
                .Where(c => c.Id == id)
                .ExecuteDeleteAsync();
        }

        public Task<bool> ExistsAsync(Guid id)
        {
            return _db.Categories
                .AnyAsync(c => c.Id == id && c.IsActive);   
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _db.Categories
                .Where(c => c.IsActive)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(Guid id)
        {
            return await _db.Categories
                .Where(c => c.Id == id && c.IsActive)
                .FirstOrDefaultAsync();
        } 
        public async Task UpdateAsync(Category category)
        {
            category.UpdatedAt = DateTime.UtcNow;
            _db.Categories.Update(category);
            await _db.SaveChangesAsync();
        }
    }
}
