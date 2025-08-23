using ecommerceAPI.src.EcommerceAPI.Domain.Entities;

namespace ecommerceAPI.src.EcommerceAPI.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetByIdAsync(Guid id);
        Task<bool> IsUniqueUsername(string username);  
        Task<User?> GetByUsernameAsync(string username);
        Task UpdateAsync(User user);
        Task DeleteAsync(Guid id);
 
        Task<User> RegisterAsync(User user);
        Task LogoutAsync(Guid userId);

        Task ChangePasswordAsync(Guid userId, string newPassword); 


    }
}
