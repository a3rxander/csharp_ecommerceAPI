using System.Security.Claims;
using Asp.Versioning;
using ecommerceAPI.src.EcommerceAPI.Application.DTOs;
using ecommerceAPI.src.EcommerceAPI.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ecommerceAPI.src.EcommerceAPI.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto registerDto)
        { 

            if (registerDto == null)
            {
                return BadRequest(new { message = "Invalid user data." });
            }

            var authResult = await _authService.RegisterAsync(registerDto);
            if (!authResult.Succeeded)
            {
                var hasDuplicateError = authResult.Errors.Any(error =>
                    error.Contains("already exists", StringComparison.OrdinalIgnoreCase));
                var statusCode = hasDuplicateError ? 409 : 400;

                return StatusCode(statusCode, new
                {
                    message = "Could not register user.",
                    errors = authResult.Errors
                });
            }

            return Ok(authResult.AuthResponse);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginDto)
        {
            if (loginDto == null)
            {
                return BadRequest(new { message = "Invalid login data." });
            }

            var authResponse = await _authService.LoginAsync(loginDto);
            if (authResponse == null)
            {
                return Unauthorized(new { message = "Invalid username or password." });
            }

            return Ok(authResponse);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return NoContent();
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        { 
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Console.WriteLine("User ID from token: " + userIdValue);
            if (userIdValue == null || !Guid.TryParse(userIdValue, out var userId))
            {
                return Unauthorized(new { message = "Invalid token." });
            }
            var userDto = await _authService.GetUserByIdAsync(userId);
            if (userDto == null)
            {
                return NotFound(new { message = "User not found." });
            }   
            return Ok(userDto);
        }

        [Authorize]
        [HttpPut("me")]
        public async Task<IActionResult> UpdateCurrentUser([FromBody] UpdateUserDto updateDto)
        {
            if (updateDto == null)
            {
                return BadRequest(new { message = "Invalid user data." });
            }

            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdValue == null || !Guid.TryParse(userIdValue, out var userId))
            {
                return Unauthorized(new { message = "Invalid token." });
            }
            var result = await _authService.UpdateUserAsync(userId, updateDto);
            if (!result)
            {
                return NotFound(new { message = "User not found." });
            }
            return NoContent();
        }

    }
}
