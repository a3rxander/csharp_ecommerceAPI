using Mapster;
using ecommerceAPI.src.EcommerceAPI.Application.DTOs;
using ecommerceAPI.src.EcommerceAPI.Application.Interfaces;
using ecommerceAPI.src.EcommerceAPI.Domain.Entities;
using ecommerceAPI.src.EcommerceAPI.Domain.Repositories;

namespace ecommerceAPI.src.EcommerceAPI.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return products.Adapt<IEnumerable<ProductDto>>();
        }

        public async Task<ProductDto?> GetProductByIdAsync(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            return product == null ? null : product.Adapt<ProductDto>();
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductDto productDto, string sellerId)
        {
            if (!await _categoryRepository.ExistsAsync(productDto.CategoryId))
            {
                throw new ArgumentException("Invalid CategoryId");
            }

            var product = productDto.Adapt<Product>();
            product.Id = Guid.NewGuid();
            product.SellerId = sellerId;
            product.CreatedAt = DateTime.UtcNow;
            product.UpdatedAt = DateTime.UtcNow;
            product.IsActive = true;

            var createdProduct = await _productRepository.AddAsync(product);
            var fullProduct = await _productRepository.GetByIdAsync(createdProduct.Id) ?? createdProduct;
            return fullProduct.Adapt<ProductDto>();
        }

        public async Task<bool> DeleteProductAsync(Guid id)
        {
            if (!await _productRepository.ExistsAsync(id))
            {
                return false;
            }
            await _productRepository.DeleteAsync(id);
            return true;
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(Guid categoryId)
        {
            var products = await _productRepository.GetProductsByCategoryAsync(categoryId);
            return products.Adapt<IEnumerable<ProductDto>>();
        }

        public async Task<bool> UpdateProductAsync(Guid id, UpdateProductDto productDto, string sellerId)
        {
            var existingProduct = await _productRepository.GetByIdAsync(id);
            if (existingProduct == null || !existingProduct.IsActive || existingProduct.SellerId != sellerId)
            {
                return false;
            }
            var updatedProduct = productDto.Adapt(existingProduct);
            updatedProduct.UpdatedAt = DateTime.UtcNow;
            await _productRepository.UpdateAsync(updatedProduct);
            return true;
        }

        public async Task<bool> UpdateProductStockAsync(Guid id, UpdateProductStockDto updateProductStockDto)
        {
            var existingProduct = await _productRepository.GetByIdAsync(id);
            if (existingProduct == null || !existingProduct.IsActive)
            {
                return false;
            }
            existingProduct.Stock = updateProductStockDto.Stock;
            existingProduct.UpdatedAt = DateTime.UtcNow;
            await _productRepository.UpdateAsync(existingProduct);
            return true;
        }

        public async Task<IEnumerable<ProductDto>> GetProductsBySellerAsync(string sellerId)
        {
            var products = await _productRepository.GetProductsBySellerAsync(sellerId);
            return products.Adapt<IEnumerable<ProductDto>>();
        }
    }
}

