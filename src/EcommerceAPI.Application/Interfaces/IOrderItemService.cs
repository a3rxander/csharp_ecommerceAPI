using ecommerceAPI.src.EcommerceAPI.Application.DTOs;

namespace ecommerceAPI.src.EcommerceAPI.Application.Interfaces
{
    public interface IOrderItemService
    {
        Task<IEnumerable<OrderItemDto>> GetItemsByOrderIdAsync(Guid orderId, Guid userId);
        Task<OrderItemDto?> GetItemByIdAsync(Guid id, Guid userId);
        Task<OrderItemDto> AddItemAsync(Guid orderId, Guid userId, CreateOrderItemDto orderItemDto);
        Task<bool> UpdateItemAsync(Guid id, Guid userId, UpdateOrderItemDto orderItemDto);
        Task<bool> DeleteItemAsync(Guid id, Guid userId);
    }
}
