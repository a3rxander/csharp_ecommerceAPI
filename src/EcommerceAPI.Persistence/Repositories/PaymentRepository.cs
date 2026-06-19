using ecommerceAPI.src.EcommerceAPI.Domain.Entities;
using ecommerceAPI.src.EcommerceAPI.Domain.Repositories;
using ecommerceAPI.src.EcommerceAPI.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace ecommerceAPI.src.EcommerceAPI.Persistence.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly EcommerceDbContext _db;

        public PaymentRepository(EcommerceDbContext context)
        {
            _db = context;
        }

        public async Task<Payment> AddAsync(Payment payment)
        {
            await _db.Payments.AddAsync(payment);
            await _db.SaveChangesAsync();
            return payment;
        }

        public async Task DeleteAsync(Guid id)
        {
            await _db.Payments
                .Where(p => p.Id == id)
                .ExecuteDeleteAsync();
        }

        public Task<bool> ExistsAsync(Guid id)
        {
            return _db.Payments
                .AnyAsync(p => p.Id == id && p.IsActive);
        }

        public async Task<IEnumerable<Payment>> GetAllAsync()
        {
            return await _db.Payments
                .Where(p => p.IsActive)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();
        }

        public async Task<Payment?> GetByIdAsync(Guid id)
        {
            return await _db.Payments
                .Where(p => p.Id == id && p.IsActive)
                .FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(Payment payment)
        {
            payment.UpdatedAt = DateTime.UtcNow;
            _db.Payments.Update(payment);
            await _db.SaveChangesAsync();
        }
    }
}
