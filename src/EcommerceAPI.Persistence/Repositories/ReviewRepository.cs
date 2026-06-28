using ecommerceAPI.src.EcommerceAPI.Domain.Entities;
using ecommerceAPI.src.EcommerceAPI.Domain.Repositories;
using ecommerceAPI.src.EcommerceAPI.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace ecommerceAPI.src.EcommerceAPI.Persistence.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly EcommerceDbContext _db;

        public ReviewRepository(EcommerceDbContext context)
        {
            _db = context;
        }

        public async Task<Review> AddAsync(Review review)
        {
            await _db.Reviews.AddAsync(review);
            await _db.SaveChangesAsync();
            return review;
        }

        public async Task DeleteAsync(Guid id)
        {
            await _db.Reviews
                .Where(r => r.Id == id)
                .ExecuteDeleteAsync();
        }

        public Task<bool> ExistsAsync(Guid id)
        {
            return _db.Reviews
                .AnyAsync(r => r.Id == id && r.IsActive);
        }

        public async Task<IEnumerable<Review>> GetAllAsync()
        {
            return await _db.Reviews
                .Where(r => r.IsActive)
                .Include(r => r.User)
                .OrderByDescending(r => r.ReviewDate)
                .ToListAsync();
        }

        public async Task<Review?> GetByIdAsync(Guid id)
        {
            return await _db.Reviews
                .Where(r => r.Id == id && r.IsActive)
                .Include(r => r.User)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Review>> GetReviewsByProductAsync(Guid productId)
        {
            return await _db.Reviews
                .Where(r => r.ProductId == productId && r.IsActive)
                .Include(r => r.User)
                .OrderByDescending(r => r.ReviewDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetReviewsByUserAsync(string userId)
        {
            return await _db.Reviews
                .Where(r => r.UserId == userId && r.IsActive)
                .Include(r => r.User)
                .OrderByDescending(r => r.ReviewDate)
                .ToListAsync();
        }

        public async Task UpdateAsync(Review review)
        {
            review.UpdatedAt = DateTime.UtcNow;
            _db.Reviews.Update(review);
            await _db.SaveChangesAsync();
        }
    }
}
