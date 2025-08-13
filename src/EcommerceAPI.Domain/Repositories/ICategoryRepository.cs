using ecommerceAPI.src.EcommerceAPI.Domain.Entities;

namespace ecommerceAPI.src.EcommerceAPI.Domain.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(Guid id); 
        Task UpdateAsync(Category category);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);

    }
}
