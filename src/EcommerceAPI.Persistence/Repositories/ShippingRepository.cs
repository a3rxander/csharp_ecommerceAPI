using ecommerceAPI.src.EcommerceAPI.Domain.Entities;
using ecommerceAPI.src.EcommerceAPI.Domain.Repositories;
using ecommerceAPI.src.EcommerceAPI.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace ecommerceAPI.src.EcommerceAPI.Persistence.Repositories
{
    public class ShippingRepository : IShippingRepository
    {
        private readonly EcommerceDbContext _db;

        public ShippingRepository(EcommerceDbContext context)
        {
            _db = context;
        }

        public async Task<Shipping> AddAsync(Shipping shipping)
        {
            await _db.Shippings.AddAsync(shipping);
            await _db.SaveChangesAsync();
            return shipping;
        }

        public async Task DeleteAsync(Guid id)
        {
            await _db.Shippings
                .Where(s => s.Id == id)
                .ExecuteDeleteAsync();
        }

        public Task<bool> ExistsAsync(Guid id)
        {
            return _db.Shippings
                .AnyAsync(s => s.Id == id && s.IsActive);
        }

        public async Task<IEnumerable<Shipping>> GetAllAsync()
        {
            return await _db.Shippings
                .Where(s => s.IsActive)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        }

        public async Task<Shipping?> GetByIdAsync(Guid id)
        {
            return await _db.Shippings
                .Where(s => s.Id == id && s.IsActive)
                .FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(Shipping shipping)
        {
            shipping.UpdatedAt = DateTime.UtcNow;
            _db.Shippings.Update(shipping);
            await _db.SaveChangesAsync();
        }
    }
}
