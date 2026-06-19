using ecommerceAPI.src.EcommerceAPI.Domain.Entities;
using ecommerceAPI.src.EcommerceAPI.Domain.Repositories;
using ecommerceAPI.src.EcommerceAPI.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace ecommerceAPI.src.EcommerceAPI.Persistence.Repositories
{
    public class StockMovementRepository : IStockMovementRepository
    {
        private readonly EcommerceDbContext _db;

        public StockMovementRepository(EcommerceDbContext context)
        {
            _db = context;
        }

        public async Task<StockMovement> AddAsync(StockMovement stockMovement)
        {
            await _db.StockMovements.AddAsync(stockMovement);
            await _db.SaveChangesAsync();
            return stockMovement;
        }

        public async Task DeleteAsync(Guid id)
        {
            await _db.StockMovements
                .Where(sm => sm.Id == id)
                .ExecuteDeleteAsync();
        }

        public Task<bool> ExistsAsync(Guid id)
        {
            return _db.StockMovements
                .AnyAsync(sm => sm.Id == id && sm.IsActive);
        }

        public async Task<IEnumerable<StockMovement>> GetAllAsync()
        {
            return await _db.StockMovements
                .Where(sm => sm.IsActive)
                .OrderByDescending(sm => sm.MovementDate)
                .ToListAsync();
        }

        public async Task<StockMovement?> GetByIdAsync(Guid id)
        {
            return await _db.StockMovements
                .Where(sm => sm.Id == id && sm.IsActive)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<StockMovement>> GetStockMovementsByProductAsync(Guid productId)
        {
            return await _db.StockMovements
                .Where(sm => sm.ProductId == productId && sm.IsActive)
                .OrderByDescending(sm => sm.MovementDate)
                .ToListAsync();
        }

        public async Task UpdateAsync(StockMovement stockMovement)
        {
            stockMovement.UpdatedAt = DateTime.UtcNow;
            _db.StockMovements.Update(stockMovement);
            await _db.SaveChangesAsync();
        }
    }
}
