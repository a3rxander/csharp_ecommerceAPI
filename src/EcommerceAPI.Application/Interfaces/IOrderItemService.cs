using ecommerceAPI.src.EcommerceAPI.Application.DTOs;

namespace ecommerceAPI.src.EcommerceAPI.Application.Interfaces
{
    public interface IOrderItemService
    {
        Task<IEnumerable<OrderItemDto>> GetItemsByOrderIdAsync(Guid orderId, string UserId);
        Task<OrderItemDto?> GetItemByIdAsync(Guid id, string UserId);
        Task<OrderItemDto> AddItemAsync(Guid orderId, string UserId, CreateOrderItemDto orderItemDto);
        Task<bool> UpdateItemAsync(Guid id, string UserId, UpdateOrderItemDto orderItemDto);
        Task<bool> DeleteItemAsync(Guid id, string UserId);
    }
}
