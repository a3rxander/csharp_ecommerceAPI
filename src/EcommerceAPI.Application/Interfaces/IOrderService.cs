using ecommerceAPI.src.EcommerceAPI.Application.DTOs;

namespace ecommerceAPI.src.EcommerceAPI.Application.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetOrdersByUserAsync(Guid userId);
        Task<OrderDto?> GetOrderByIdAsync(Guid id, Guid userId);
        Task<OrderDto> CreateOrderAsync(Guid userId, CreateOrderDto orderDto);
        Task<bool> UpdateOrderAsync(Guid id, Guid userId, UpdateOrderDto orderDto);
        Task<bool> DeleteOrderAsync(Guid id, Guid userId);
    }
}
