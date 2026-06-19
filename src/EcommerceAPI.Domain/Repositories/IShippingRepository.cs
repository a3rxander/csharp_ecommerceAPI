using ecommerceAPI.src.EcommerceAPI.Domain.Entities;

namespace ecommerceAPI.src.EcommerceAPI.Domain.Repositories
{
    public interface IShippingRepository
    {
        Task<IEnumerable<Shipping>> GetAllAsync();
        Task<Shipping?> GetByIdAsync(Guid id);
        Task<Shipping> AddAsync(Shipping shipping);
        Task UpdateAsync(Shipping shipping);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
}
