using ecommerceAPI.src.EcommerceAPI.Application.DTOs;

namespace ecommerceAPI.src.EcommerceAPI.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto?> RegisterAsync(RegisterUserDto registerDto);
        Task<AuthResponseDto?> LoginAsync(LoginUserDto loginDto);
        Task LogoutAsync();
        Task<bool> UpdateUserAsync(Guid id, UpdateUserDto updateDto);
    }

}