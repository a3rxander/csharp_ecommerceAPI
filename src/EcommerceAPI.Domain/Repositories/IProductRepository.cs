using ecommerceAPI.src.EcommerceAPI.Domain.Entities;
using ecommerceAPI.src.EcommerceAPI.Application.DTOs;

namespace ecommerceAPI.src.EcommerceAPI.Domain.Repositories
{
    public interface IProductRepository
    {
        Task<(IEnumerable<Product> Products, int Total)> GetAllAsync(ProductQueryParams queryParams);
        Task<Product?> GetByIdAsync(Guid id);
        Task<Product> AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);

        Task<IEnumerable<Product>> GetProductsByCategoryAsync(Guid categoryId);
        Task<IEnumerable<Product>> GetProductsBySellerAsync(string sellerId);
    }
}
