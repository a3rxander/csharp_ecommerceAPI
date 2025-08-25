using ecommerceAPI.src.EcommerceAPI.Application.DTOs;
using ecommerceAPI.src.EcommerceAPI.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ecommerceAPI.src.EcommerceAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrdersForUser()
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdValue == null || !Guid.TryParse(userIdValue, out var userId))
            {
                return Unauthorized("Invalid token.");
            }

            var orders = await _orderService.GetOrdersByUserAsync(userId);
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdValue == null || !Guid.TryParse(userIdValue, out var userId))
            {
                return Unauthorized("Invalid token.");
            }

            var order = await _orderService.GetOrderByIdAsync(id, userId);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto orderDto)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdValue == null || !Guid.TryParse(userIdValue, out var userId))
            {
                return Unauthorized("Invalid token.");
            }

            var createdOrder = await _orderService.CreateOrderAsync(userId, orderDto);
            return CreatedAtAction(nameof(GetOrderById), new { id = createdOrder.Id }, createdOrder);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(Guid id, [FromBody] UpdateOrderDto orderDto)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdValue == null || !Guid.TryParse(userIdValue, out var userId))
            {
                return Unauthorized("Invalid token.");
            }

            var updated = await _orderService.UpdateOrderAsync(id, userId, orderDto);
            if (!updated)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdValue == null || !Guid.TryParse(userIdValue, out var userId))
            {
                return Unauthorized("Invalid token.");
            }

            var deleted = await _orderService.DeleteOrderAsync(id, userId);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
