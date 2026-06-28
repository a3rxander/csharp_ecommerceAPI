using ecommerceAPI.src.EcommerceAPI.Application.DTOs;

namespace ecommerceAPI.src.EcommerceAPI.Application.Interfaces
{
    public interface ICartService
    {
        Task<IEnumerable<CartDto>> GetAllCartsAsync();
        Task<CartDto?> GetCartByIdAsync(Guid id);
        Task<CartDto?> GetCartByUserAsync(string userId);
        Task<CartDto> CreateCartAsync(string userId);
        Task<CartDto> AddItemAsync(string userId, AddCartItemDto itemDto);
        Task<bool> UpdateItemAsync(string userId, Guid itemId, UpdateCartItemDto itemDto);
        Task<bool> RemoveItemAsync(string userId, Guid itemId);
        Task<bool> ClearCartAsync(string userId);
        Task<bool> DeleteCartAsync(string userId);
    }
}
