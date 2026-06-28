using ecommerceAPI.src.EcommerceAPI.Domain.Entities;

namespace ecommerceAPI.src.EcommerceAPI.Domain.Repositories
{
    public interface ICartRepository
    {
        Task<IEnumerable<Cart>> GetAllAsync();
        Task<Cart?> GetByIdAsync(Guid id);
        Task<Cart?> GetByUserIdAsync(string userId);
        Task<Cart> AddAsync(Cart cart);
        Task UpdateAsync(Cart cart);
        Task DeleteAsync(Guid id);
        Task DeleteItemAsync(Guid itemId);
        Task ClearItemsAsync(Guid cartId);
        Task<bool> ExistsAsync(Guid id);
    }
}
