using ecommerceAPI.src.EcommerceAPI.Application.DTOs;
using ecommerceAPI.src.EcommerceAPI.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Asp.Versioning;

namespace ecommerceAPI.src.EcommerceAPI.API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]

    public class ProductImageController : ControllerBase
    {
        private readonly IProductImageService _productImageService;
        public ProductImageController(IProductImageService productImageService)
        {
            _productImageService = productImageService;
        }
        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetImagesByProductId(Guid productId)
        {
            var images = await _productImageService.GetImagesByProductIdAsync(productId);
            return Ok(images);
        }
        [Authorize(Roles = "Seller")]
        [HttpPost]
        public async Task<IActionResult> AddProductImage([FromForm] CreateProductImageDto imageDto)
        {
            
            if (imageDto == null || imageDto.ImageFile == null)
            {
                return BadRequest("Image file is required.");
            }
            var createdImage = await _productImageService.CreateProductImageAsync(imageDto);
            if (createdImage == null)
            {
                return BadRequest("Failed to add product image.");
            }
            return CreatedAtAction(nameof(GetImagesByProductId), new { id = createdImage.Id }, createdImage);
        }
        [Authorize(Roles = "Seller")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductImage(Guid id)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdValue == null || !Guid.TryParse(userIdValue, out var userId))
            {
                return Unauthorized("Invalid token.");
            }
            await _productImageService.DeleteProductImageAsync(id, userIdValue);
            return NoContent();
        }
    }
}