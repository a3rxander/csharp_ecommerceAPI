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
    [Authorize(Roles = "Admin,Seller")]
    public class StockMovementsController : ControllerBase
    {
        private readonly IStockMovementService _stockMovementService;

        public StockMovementsController(IStockMovementService stockMovementService)
        {
            _stockMovementService = stockMovementService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StockMovementDto>>> GetAllStockMovements()
        {
            var stockMovements = await _stockMovementService.GetAllStockMovementsAsync();
            return Ok(stockMovements);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<StockMovementDto>> GetStockMovementById(Guid id)
        {
            var stockMovement = await _stockMovementService.GetStockMovementByIdAsync(id);
            if (stockMovement == null)
            {
                return NotFound();
            }

            return Ok(stockMovement);
        }

        [HttpGet("product/{productId:guid}")]
        public async Task<ActionResult<IEnumerable<StockMovementDto>>> GetStockMovementsByProduct(Guid productId)
        {
            var stockMovements = await _stockMovementService.GetStockMovementsByProductAsync(productId);
            return Ok(stockMovements);
        }

        [HttpPost]
        public async Task<ActionResult<StockMovementDto>> CreateStockMovement([FromBody] CreateStockMovementDto stockMovementDto)
        {
            if (stockMovementDto == null)
            {
                return BadRequest("Stock movement data is required.");
            }

            try
            {
                var createdStockMovement = await _stockMovementService.CreateStockMovementAsync(stockMovementDto);
                return CreatedAtAction(nameof(GetStockMovementById), new { id = createdStockMovement.Id, version = "1.0" }, createdStockMovement);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateStockMovement(Guid id, [FromBody] UpdateStockMovementDto stockMovementDto)
        {
            if (stockMovementDto == null)
            {
                return BadRequest("Stock movement data is required.");
            }

            var updated = await _stockMovementService.UpdateStockMovementAsync(id, stockMovementDto);
            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteStockMovement(Guid id)
        {
            var deleted = await _stockMovementService.DeleteStockMovementAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
