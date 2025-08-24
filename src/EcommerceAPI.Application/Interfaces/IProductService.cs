using ecommerceAPI.src.EcommerceAPI.Application.DTOs;

namespace ecommerceAPI.src.EcommerceAPI.Application.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<ProductDto?> GetProductByIdAsync(Guid id);
        Task<ProductDto> CreateProductAsync(CreateProductDto productDto);
        Task<bool> UpdateProductAsync(Guid id, UpdateProductDto productDto);
        Task<bool> DeleteProductAsync(Guid id);
        Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(Guid categoryId);
        Task<bool> UpdateProductStockAsync(Guid id, UpdateProductStockDto updateProductStockDto);
        Task<IEnumerable<ProductDto>> GetProductsBySellerAsync(Guid sellerId);

    }
}
