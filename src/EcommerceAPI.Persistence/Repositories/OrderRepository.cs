using ecommerceAPI.src.EcommerceAPI.Domain.Entities;
using ecommerceAPI.src.EcommerceAPI.Domain.Repositories;
using ecommerceAPI.src.EcommerceAPI.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace ecommerceAPI.src.EcommerceAPI.Persistence.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly EcommerceDbContext _db;

        public OrderRepository(EcommerceDbContext context)
        {
            _db = context;
        }

        public async Task<Order> AddAsync(Order order)
        {
            await _db.Orders.AddAsync(order);
            await _db.SaveChangesAsync();
            return order;
        }

        public async Task DeleteAsync(Guid id)
        {
            await _db.Orders
                .Where(o => o.Id == id)
                .ExecuteDeleteAsync();
        }

        public Task<bool> ExistsAsync(Guid id)
        {
            return _db.Orders
                .AnyAsync(o => o.Id == id && o.IsActive);
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _db.Orders
                .Where(o => o.IsActive)
                .Include(o => o.Items)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(Guid id)
        {
            return await _db.Orders
                .Where(o => o.Id == id && o.IsActive)
                .Include(o => o.Items)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserAsync(string UserId)
        {
            return await _db.Orders
                .Where(o => o.UserId == UserId && o.IsActive)
                .Include(o => o.Items)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task UpdateAsync(Order order)
        {
            order.UpdatedAt = DateTime.UtcNow;
            _db.Orders.Update(order);
            await _db.SaveChangesAsync();
        }
    }
}
