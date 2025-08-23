
using System.Data.Entity;
using ecommerceAPI.src.EcommerceAPI.Domain.Entities;
using ecommerceAPI.src.EcommerceAPI.Domain.Repositories;
using ecommerceAPI.src.EcommerceAPI.Persistence.Data;

namespace ecommerceAPI.src.EcommerceAPI.Persistence.Repositories
{

    public class UserRepository : IUserRepository
    {
        private readonly EcommerceDbContext _db;

        public UserRepository(EcommerceDbContext context)
        {
            _db = context;
        } 

        public async Task ChangePasswordAsync(Guid userId, string newPassword)
        {
            var user = await _db.Users.Where(u => u.Id == userId && u.IsActive).FirstOrDefaultAsync();
            if (user != null)
            {
                user.PasswordHash = newPassword; // In real application, hash the password
                user.UpdatedAt = DateTime.UtcNow;
                _db.Users.Update(user);
                await _db.SaveChangesAsync();
            } 
        }

        public async Task DeleteAsync(Guid id)
        {
            //get user and update isActive to false
            var user = await _db.Users.Where(u => u.Id == id && u.IsActive).FirstOrDefaultAsync();
            if (user != null)
            {
                user.IsActive = false;
                user.UpdatedAt = DateTime.UtcNow;
                _db.Users.Update(user);
                await _db.SaveChangesAsync();
            }
        } 

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _db.Users
                .Where(u => u.IsActive)
                .OrderBy(u => u.Username)
                .ToListAsync();
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _db.Users.Where(u => u.Id == id && u.IsActive).FirstOrDefaultAsync();
        }

        public async Task<bool> IsUniqueUsername(string username)
        {
            return await _db.Users.Where(u => u.Username == username).AnyAsync();
        }

        public Task LogoutAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<User> RegisterAsync(User user)
        {
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
            return user;
        }

        public async Task UpdateAsync(User user)
        {
            user.UpdatedAt = DateTime.UtcNow;
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }
    }

}