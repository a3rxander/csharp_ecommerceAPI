using ecommerceAPI.src.EcommerceAPI.Application.DTOs;

namespace ecommerceAPI.src.EcommerceAPI.Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto?> GetUserByIdAsync(Guid id);
        Task<UserDto> CreateUserAsync(RegisterUserDto userDto);
        Task<bool> UpdateUserAsync(Guid id, UpdateUserDto userDto);
        Task<bool> DeleteUserAsync(Guid id);
    }
}
