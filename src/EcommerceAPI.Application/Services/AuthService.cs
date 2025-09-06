using Mapster;
using ecommerceAPI.src.EcommerceAPI.Application.DTOs;
using ecommerceAPI.src.EcommerceAPI.Application.Interfaces;
using ecommerceAPI.src.EcommerceAPI.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using ecommerceAPI.src.EcommerceAPI.Domain.Constants;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace ecommerceAPI.src.EcommerceAPI.Application.Services
{

    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration, UserManager<User> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }
        public async Task<AuthResponseDto?> LoginAsync(LoginUserDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.Username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                return null;
            }
            
            var token = GenerateJwtToken(user);
            var userDto = user.Adapt<UserDto>();
            return new AuthResponseDto
            {
                Token = token,
                User = userDto
            };
        }

        public Task LogoutAsync()
        {
            // For JWT, logout is handled client-side by discarding the token.
            return Task.CompletedTask;
        }

        public async Task<AuthResponseDto?> RegisterAsync(RegisterUserDto registerDto)
        {
            var ifExistUsername = await _userManager.FindByNameAsync(registerDto.Username);
            if (ifExistUsername != null) return null;
            if (string.IsNullOrEmpty(registerDto.Email))
            {
                return null;
            }
            var ifExistEmail = await _userManager.FindByEmailAsync(registerDto.Email);
            if (ifExistEmail != null) return null;

            //valid Role selection got error
            if (registerDto.Role != UserRoles.Customer && registerDto.Role != UserRoles.Seller)
            {
                return null;
            }
            var user = registerDto.Adapt<User>();
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow; 
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                return null;
            }
            var userDto = user.Adapt<UserDto>();
            var token = GenerateJwtToken(user);
            return new AuthResponseDto
            {
                Token = token,
                User = userDto
            };
        }

        public async Task<bool> UpdateUserAsync(Guid id, UpdateUserDto updateDto)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return false;
            user.FirstName = updateDto.FirstName ?? user.FirstName;
            user.LastName = updateDto.LastName ?? user.LastName;
            user.Email = updateDto.Email ?? user.Email;
            user.UpdatedAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);
            return true;
            
        }

        public async Task<UserDto?> GetUserByIdAsync(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return null;
            return user.Adapt<UserDto>();
        }

        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName ?? string.Empty),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}