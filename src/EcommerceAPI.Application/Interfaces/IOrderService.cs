using ecommerceAPI.src.EcommerceAPI.Application.DTOs;

namespace ecommerceAPI.src.EcommerceAPI.Application.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetOrdersByUserAsync(string UserId);
        Task<OrderDto?> GetOrderByIdAsync(Guid id, string UserId);
        Task<OrderDto> CreateOrderAsync(string UserId, CreateOrderDto orderDto);
        Task<bool> UpdateOrderAsync(Guid id, string UserId, UpdateOrderDto orderDto);
        Task<bool> DeleteOrderAsync(Guid id, string UserId);
    }
}
