using ecommerceAPI.src.EcommerceAPI.Application.DTOs;

namespace ecommerceAPI.src.EcommerceAPI.Application.Interfaces
{
    public interface IProductImageService
    {
        Task<IEnumerable<ProductImageDto>> GetImagesByProductIdAsync(Guid productId);
        Task<ProductImageDto?> GetByIdAsync(Guid id);
        Task<ProductImageDto> CreateProductImageAsync(CreateProductImageDto createProductImageDto);
        Task UpdateProductImageAsync(Guid id, UpdateProductImageDto updateProductImageDto);
        Task DeleteProductImageAsync(Guid id, string userId);
    }
}