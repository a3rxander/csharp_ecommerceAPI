using ecommerceAPI.src.EcommerceAPI.Domain.Entities;
using ecommerceAPI.src.EcommerceAPI.Domain.Repositories;
using ecommerceAPI.src.EcommerceAPI.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace ecommerceAPI.src.EcommerceAPI.Persistence.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly EcommerceDbContext _db;

        public OrderItemRepository(EcommerceDbContext context)
        {
            _db = context;
        }

        public async Task<OrderItem> AddAsync(OrderItem item)
        {
            await _db.OrderItems.AddAsync(item);
            await _db.SaveChangesAsync();
            return item;
        }

        public async Task DeleteAsync(Guid id)
        {
            await _db.OrderItems
                .Where(i => i.Id == id)
                .ExecuteDeleteAsync();
        }

        public Task<bool> ExistsAsync(Guid id)
        {
            return _db.OrderItems
                .AnyAsync(i => i.Id == id && i.IsActive);
        }

        public async Task<OrderItem?> GetByIdAsync(Guid id)
        {
            return await _db.OrderItems
                .Where(i => i.Id == id && i.IsActive)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<OrderItem>> GetItemsByOrderIdAsync(Guid orderId)
        {
            return await _db.OrderItems
                .Where(i => i.OrderId == orderId && i.IsActive)
                .ToListAsync();
        }

        public async Task UpdateAsync(OrderItem item)
        {
            item.UpdatedAt = DateTime.UtcNow;
            _db.OrderItems.Update(item);
            await _db.SaveChangesAsync();
        }
    }
}
