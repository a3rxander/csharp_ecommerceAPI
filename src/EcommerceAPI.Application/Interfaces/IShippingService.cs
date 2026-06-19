using ecommerceAPI.src.EcommerceAPI.Application.DTOs;

namespace ecommerceAPI.src.EcommerceAPI.Application.Interfaces
{
    public interface IShippingService
    {
        Task<IEnumerable<ShippingDto>> GetAllShippingsAsync();
        Task<ShippingDto?> GetShippingByIdAsync(Guid id);
        Task<ShippingDto> CreateShippingAsync(CreateShippingDto shippingDto);
        Task<bool> UpdateShippingAsync(Guid id, UpdateShippingDto shippingDto);
        Task<bool> DeleteShippingAsync(Guid id);
    }
}
