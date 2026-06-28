using ecommerceAPI.src.EcommerceAPI.Domain.Entities;
using ecommerceAPI.src.EcommerceAPI.Domain.Repositories;
using ecommerceAPI.src.EcommerceAPI.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace ecommerceAPI.src.EcommerceAPI.Persistence.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly EcommerceDbContext _db;

        public CartRepository(EcommerceDbContext context)
        {
            _db = context;
        }

        public async Task<Cart> AddAsync(Cart cart)
        {
            await _db.Carts.AddAsync(cart);
            await _db.SaveChangesAsync();
            return cart;
        }

        public async Task ClearItemsAsync(Guid cartId)
        {
            await _db.CartItems
                .Where(ci => ci.CartId == cartId)
                .ExecuteDeleteAsync();

            await _db.Carts
                .Where(c => c.Id == cartId)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(c => c.UpdatedAt, DateTime.UtcNow));
        }

        public async Task DeleteAsync(Guid id)
        {
            await _db.Carts
                .Where(c => c.Id == id)
                .ExecuteDeleteAsync();
        }

        public async Task DeleteItemAsync(Guid itemId)
        {
            var cartId = await _db.CartItems
                .Where(ci => ci.Id == itemId)
                .Select(ci => (Guid?)ci.CartId)
                .FirstOrDefaultAsync();

            await _db.CartItems
                .Where(ci => ci.Id == itemId)
                .ExecuteDeleteAsync();

            if (cartId.HasValue)
            {
                await _db.Carts
                    .Where(c => c.Id == cartId.Value)
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(c => c.UpdatedAt, DateTime.UtcNow));
            }
        }

        public Task<bool> ExistsAsync(Guid id)
        {
            return _db.Carts
                .AnyAsync(c => c.Id == id && c.IsActive);
        }

        public async Task<IEnumerable<Cart>> GetAllAsync()
        {
            return await CartQuery()
                .OrderByDescending(c => c.UpdatedAt)
                .ToListAsync();
        }

        public async Task<Cart?> GetByIdAsync(Guid id)
        {
            return await CartQuery()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Cart?> GetByUserIdAsync(string userId)
        {
            return await CartQuery()
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task UpdateAsync(Cart cart)
        {
            cart.UpdatedAt = DateTime.UtcNow;

            var entry = _db.Entry(cart);
            if (entry.State == EntityState.Detached)
            {
                _db.Carts.Attach(cart);
                _db.Entry(cart).Property(c => c.UpdatedAt).IsModified = true;
            }

            var itemIds = cart.Items
                .Select(item => item.Id)
                .Where(id => id != Guid.Empty)
                .ToList();

            var existingItemIds = await _db.CartItems
                .Where(item => itemIds.Contains(item.Id))
                .Select(item => item.Id)
                .ToListAsync();

            foreach (var item in cart.Items.Where(item => !existingItemIds.Contains(item.Id)))
            {
                _db.Entry(item).State = EntityState.Added;

                if (item.Product != null)
                {
                    _db.Entry(item.Product).State = EntityState.Unchanged;
                }
            }

            await _db.SaveChangesAsync();
        }

        private IQueryable<Cart> CartQuery()
        {
            return _db.Carts
                .Where(c => c.IsActive)
                .Include(c => c.Items.Where(ci => ci.IsActive))
                    .ThenInclude(ci => ci.Product)
                        .ThenInclude(p => p.ProductImages);
        }
    }
}
