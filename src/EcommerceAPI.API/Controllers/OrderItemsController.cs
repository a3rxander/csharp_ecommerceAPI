using ecommerceAPI.src.EcommerceAPI.Application.DTOs;
using ecommerceAPI.src.EcommerceAPI.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ecommerceAPI.src.EcommerceAPI.API.Controllers
{
    [ApiController]
    [Route("api/orders/{orderId}/items")]
    [Authorize]
    public class OrderItemsController : ControllerBase
    {
        private readonly IOrderItemService _orderItemService;

        public OrderItemsController(IOrderItemService orderItemService)
        {
            _orderItemService = orderItemService;
        }

        [HttpGet]
        public async Task<IActionResult> GetItems(Guid orderId)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdValue == null || !Guid.TryParse(userIdValue, out var userId))
            {
                return Unauthorized("Invalid token.");
            }

            var items = await _orderItemService.GetItemsByOrderIdAsync(orderId, userId);
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetItem(Guid orderId, Guid id)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdValue == null || !Guid.TryParse(userIdValue, out var userId))
            {
                return Unauthorized("Invalid token.");
            }

            var item = await _orderItemService.GetItemByIdAsync(id, userId);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> AddItem(Guid orderId, [FromBody] CreateOrderItemDto orderItemDto)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdValue == null || !Guid.TryParse(userIdValue, out var userId))
            {
                return Unauthorized("Invalid token.");
            }

            var createdItem = await _orderItemService.AddItemAsync(orderId, userId, orderItemDto);
            return CreatedAtAction(nameof(GetItem), new { orderId = orderId, id = createdItem.Id }, createdItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(Guid orderId, Guid id, [FromBody] UpdateOrderItemDto orderItemDto)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdValue == null || !Guid.TryParse(userIdValue, out var userId))
            {
                return Unauthorized("Invalid token.");
            }

            var updated = await _orderItemService.UpdateItemAsync(id, userId, orderItemDto);
            if (!updated)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(Guid orderId, Guid id)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdValue == null || !Guid.TryParse(userIdValue, out var userId))
            {
                return Unauthorized("Invalid token.");
            }

            var deleted = await _orderItemService.DeleteItemAsync(id, userId);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
