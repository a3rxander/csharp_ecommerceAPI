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
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
        [Authorize(Roles = "Seller")]
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto productDto)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdValue == null || !Guid.TryParse(userIdValue, out var userId))
            {
                return Unauthorized("Invalid token.");
            }
            if (productDto == null)
            {
                return BadRequest("Product data is required.");
            }
            var createdProduct = await _productService.CreateProductAsync(productDto, userIdValue);
            if (createdProduct == null)
            {
                return BadRequest("Failed to create product.");
            }
            return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.Id, version = "1.0" }, createdProduct);
        }
        [Authorize(Roles = "Seller")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] UpdateProductDto productDto)
        {
            if (productDto == null)
            {
                return BadRequest("Product data is required.");
            }
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdValue == null || !Guid.TryParse(userIdValue, out var userId))
            {
                return Unauthorized("Invalid token.");
            }
            var existingProduct = await _productService.GetProductByIdAsync(id);
            if (existingProduct == null)
            {
                return NotFound("Product not found.");
            }
            if (existingProduct.SellerId != userIdValue)
            {
                return Forbid();
            }
            var updated = await _productService.UpdateProductAsync(id, productDto, userIdValue);
            if (!updated)
            {
                return NotFound("Product not found or update failed.");
            }
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var deleted = await _productService.DeleteProductAsync(id);
            if (!deleted)
            {
                return NotFound("Product not found.");
            }
            return NoContent();
        }
        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetProductsByCategory(Guid categoryId)
        {
            var products = await _productService.GetProductsByCategoryAsync(categoryId);
            if (products == null || !products.Any())
            {
                return NotFound("No products found for this category.");
            }
            return Ok(products);
        }
        [HttpGet("seller/{sellerId}")]
        public async Task<IActionResult> GetProductsBySeller(string sellerId)
        {
            var products = await _productService.GetProductsBySellerAsync(sellerId);
            if (products == null || !products.Any())
            {
                return NotFound("No products found for this seller.");
            }
            return Ok(products);
        }
        [HttpPut("{id}/stock")]
        public async Task<IActionResult> UpdateProductStock(Guid id, [FromBody] UpdateProductStockDto updateProductStockDto)
        {
            if (updateProductStockDto == null)
            {
                return BadRequest("Stock data is required.");
            }
            var updated = await _productService.UpdateProductStockAsync(id, updateProductStockDto);
            if (!updated)
            {
                return NotFound("Product not found or stock update failed.");
            }
            return NoContent();
        }
    }
}
