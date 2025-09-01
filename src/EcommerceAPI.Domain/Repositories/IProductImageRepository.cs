using ecommerceAPI.src.EcommerceAPI.Domain.Entities;

namespace ecommerceAPI.src.EcommerceAPI.Domain.Repositories
{
    public interface IProductImageRepository
    {
        Task<IEnumerable<ProductImage>> GetImagesByProductIdAsync(Guid productId);
        Task<ProductImage?> GetByIdAsync(Guid id);
        Task<ProductImage> AddAsync(ProductImage productImage);
        Task UpdateAsync(ProductImage productImage);
        Task DeleteAsync(Guid id);
    }
}