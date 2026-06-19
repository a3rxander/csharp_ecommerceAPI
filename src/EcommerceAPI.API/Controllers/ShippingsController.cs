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
    [Authorize(Roles = "Admin")]
    public class ShippingsController : ControllerBase
    {
        private readonly IShippingService _shippingService;

        public ShippingsController(IShippingService shippingService)
        {
            _shippingService = shippingService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShippingDto>>> GetAllShippings()
        {
            var shippings = await _shippingService.GetAllShippingsAsync();
            return Ok(shippings);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ShippingDto>> GetShippingById(Guid id)
        {
            var shipping = await _shippingService.GetShippingByIdAsync(id);
            if (shipping == null)
            {
                return NotFound();
            }

            return Ok(shipping);
        }

        [HttpPost]
        public async Task<ActionResult<ShippingDto>> CreateShipping([FromBody] CreateShippingDto shippingDto)
        {
            if (shippingDto == null)
            {
                return BadRequest("Shipping data is required.");
            }

            try
            {
                var createdShipping = await _shippingService.CreateShippingAsync(shippingDto);
                return CreatedAtAction(nameof(GetShippingById), new { id = createdShipping.Id, version = "1.0" }, createdShipping);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateShipping(Guid id, [FromBody] UpdateShippingDto shippingDto)
        {
            if (shippingDto == null)
            {
                return BadRequest("Shipping data is required.");
            }

            var updated = await _shippingService.UpdateShippingAsync(id, shippingDto);
            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteShipping(Guid id)
        {
            var deleted = await _shippingService.DeleteShippingAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
