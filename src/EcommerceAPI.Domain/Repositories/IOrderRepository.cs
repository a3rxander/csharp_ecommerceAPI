using ecommerceAPI.src.EcommerceAPI.Domain.Entities;

namespace ecommerceAPI.src.EcommerceAPI.Domain.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<IEnumerable<Order>> GetOrdersByUserAsync(string UserId);
        Task<Order?> GetByIdAsync(Guid id);
        Task<Order> AddAsync(Order order);
        Task UpdateAsync(Order order);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
}
