using ecommerceAPI.src.EcommerceAPI.Application.DTOs;
using ecommerceAPI.src.EcommerceAPI.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ecommerceAPI.src.EcommerceAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto productDto)
        {
            if (productDto == null)
            {
                return BadRequest("Product data is required.");
            }
            var createdProduct = await _productService.CreateProductAsync(productDto);
            if (createdProduct == null)
            {
                return BadRequest("Failed to create product.");
            }
            return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.Id }, createdProduct);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] UpdateProductDto productDto)
        {
            if (productDto == null)
            {
                return BadRequest("Product data is required.");
            }
            var updated = await _productService.UpdateProductAsync(id, productDto);
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
        public async Task<IActionResult> GetProductsBySeller(Guid sellerId)
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
