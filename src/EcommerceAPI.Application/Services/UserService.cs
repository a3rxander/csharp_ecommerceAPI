using Mapster;
using ecommerceAPI.src.EcommerceAPI.Application.DTOs;
using ecommerceAPI.src.EcommerceAPI.Application.Interfaces;
using ecommerceAPI.src.EcommerceAPI.Domain.Entities;
using ecommerceAPI.src.EcommerceAPI.Domain.Repositories;

namespace ecommerceAPI.src.EcommerceAPI.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto> CreateUserAsync(RegisterUserDto userDto)
        {
            var user = userDto.Adapt<User>();
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;
            user.IsActive = true;

            var createdUser = await _userRepository.AddAsync(user, userDto.Password);
            return createdUser.Adapt<UserDto>();
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            if (!await _userRepository.ExistsAsync(id))
            {
                return false;
            }

            await _userRepository.DeleteAsync(id);
            return true;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Adapt<IEnumerable<UserDto>>();
        }

        public async Task<UserDto?> GetUserByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user == null ? null : user.Adapt<UserDto>();
        }

        public async Task<bool> UpdateUserAsync(Guid id, UpdateUserDto userDto)
        {
            var existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser == null || !existingUser.IsActive)
            {
                return false;
            }

            existingUser.FirstName = userDto.FirstName ?? existingUser.FirstName;
            existingUser.LastName = userDto.LastName ?? existingUser.LastName;
            existingUser.Email = userDto.Email ?? existingUser.Email;
            existingUser.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(existingUser);
            return true;
        }
    }
}
