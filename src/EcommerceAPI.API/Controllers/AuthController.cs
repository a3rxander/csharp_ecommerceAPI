using System.Security.Claims;
using ecommerceAPI.src.EcommerceAPI.Application.DTOs;
using ecommerceAPI.src.EcommerceAPI.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ecommerceAPI.src.EcommerceAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
                return BadRequest("Invalid user data.");
            }

            var authResponse = await _authService.RegisterAsync(registerDto);
            if (authResponse == null)
            {
                return Conflict("Got error while registering user.");
            }

            return Ok(authResponse);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginDto)
        {
            if (loginDto == null)
            {
                return BadRequest("Invalid login data.");
            }

            var authResponse = await _authService.LoginAsync(loginDto);
            if (authResponse == null)
            {
                return Unauthorized("Invalid username or password.");
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
                return Unauthorized("Invalid token.");
            }
            var userDto = await _authService.GetUserByIdAsync(userId);
            if (userDto == null)
            {
                return NotFound("User not found.");
            }   
            return Ok(userDto);
        }

        [Authorize]
        [HttpPut("me")]
        public async Task<IActionResult> UpdateCurrentUser([FromBody] UpdateUserDto updateDto)
        {
            if (updateDto == null)
            {
                return BadRequest("Invalid user data.");
            }

            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdValue == null || !Guid.TryParse(userIdValue, out var userId))
            {
                return Unauthorized("Invalid token.");
            }
            var result = await _authService.UpdateUserAsync(userId, updateDto);
            if (!result)
            {
                return NotFound("User not found.");
            }
            return NoContent();
        }

    }
}