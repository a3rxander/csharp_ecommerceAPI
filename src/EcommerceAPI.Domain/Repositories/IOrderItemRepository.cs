using ecommerceAPI.src.EcommerceAPI.Domain.Entities;

namespace ecommerceAPI.src.EcommerceAPI.Domain.Repositories
{
    public interface IOrderItemRepository
    {
        Task<IEnumerable<OrderItem>> GetItemsByOrderIdAsync(Guid orderId);
        Task<OrderItem?> GetByIdAsync(Guid id);
        Task<OrderItem> AddAsync(OrderItem item);
        Task UpdateAsync(OrderItem item);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
}
