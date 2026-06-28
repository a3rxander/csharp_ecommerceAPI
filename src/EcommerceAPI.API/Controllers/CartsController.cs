using System.Security.Claims;
using Asp.Versioning;
using ecommerceAPI.src.EcommerceAPI.Application.DTOs;
using ecommerceAPI.src.EcommerceAPI.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ecommerceAPI.src.EcommerceAPI.API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Authorize]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartsController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<CartDto>>> GetAllCarts()
        {
            var carts = await _cartService.GetAllCartsAsync();
            return Ok(carts);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CartDto>> GetCartById(Guid id)
        {
            var cart = await _cartService.GetCartByIdAsync(id);
            if (cart == null)
            {
                return NotFound();
            }

            return Ok(cart);
        }

        [HttpGet]
        public async Task<ActionResult<CartDto>> GetCurrentUserCart()
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized("Invalid token.");
            }

            var cart = await _cartService.GetCartByUserAsync(userId);
            return Ok(cart ?? await _cartService.CreateCartAsync(userId));
        }

        [HttpPost("items")]
        public async Task<ActionResult<CartDto>> AddItem([FromBody] AddCartItemDto itemDto)
        {
            if (itemDto == null)
            {
                return BadRequest("Cart item data is required.");
            }

            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized("Invalid token.");
            }

            try
            {
                var cart = await _cartService.AddItemAsync(userId, itemDto);
                return Ok(cart);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("items/{itemId:guid}")]
        public async Task<IActionResult> UpdateItem(Guid itemId, [FromBody] UpdateCartItemDto itemDto)
        {
            if (itemDto == null)
            {
                return BadRequest("Cart item data is required.");
            }

            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized("Invalid token.");
            }

            try
            {
                var updated = await _cartService.UpdateItemAsync(userId, itemId, itemDto);
                if (!updated)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("items/{itemId:guid}")]
        public async Task<IActionResult> RemoveItem(Guid itemId)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized("Invalid token.");
            }

            var removed = await _cartService.RemoveItemAsync(userId, itemId);
            if (!removed)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> ClearCart()
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized("Invalid token.");
            }

            var cleared = await _cartService.ClearCartAsync(userId);
            if (!cleared)
            {
                return NotFound();
            }

            return NoContent();
        }

        private string? GetCurrentUserId()
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(userIdValue, out _) ? userIdValue : null;
        }
    }
}
