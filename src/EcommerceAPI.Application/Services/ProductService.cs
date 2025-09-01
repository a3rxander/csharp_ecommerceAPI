using AutoMapper;
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
        private readonly IMapper _mapper;
        public ProductService(IProductRepository productRepository, IMapper mapper, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }


        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<ProductDto?> GetProductByIdAsync(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            return product == null ? null : _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductDto productDto, string sellerId)
        {
            //validate category exists can be added here
            if(!await _categoryRepository.ExistsAsync(productDto.CategoryId))
            {
                throw new ArgumentException("Invalid CategoryId");
            }
            var product = _mapper.Map<Product>(productDto);
            product.Id = Guid.NewGuid();
            product.SellerId = sellerId;
            product.CreatedAt = DateTime.UtcNow;
            product.UpdatedAt = DateTime.UtcNow;
            product.IsActive = true;
            var createdProduct = await _productRepository.AddAsync(product);

            // Reload with navigation properties (Category, Seller) to populate mapping fields
            var fullProduct = await _productRepository.GetByIdAsync(createdProduct.Id) ?? createdProduct;
            return _mapper.Map<ProductDto>(fullProduct);
        }

        public async Task<bool> DeleteProductAsync(Guid id)
        {
            if (!await _productRepository.ExistsAsync(id))
            {
                return false; // Product does not exist
            }
            await _productRepository.DeleteAsync(id);
            return true;
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(Guid categoryId)
        {
            var products = await _productRepository.GetProductsByCategoryAsync(categoryId);
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<bool> UpdateProductAsync(Guid id, UpdateProductDto productDto, string sellerId)
        {
            var existingProduct = await _productRepository.GetByIdAsync(id);
            if (existingProduct == null || !existingProduct.IsActive || existingProduct.SellerId != sellerId)
            {
                return false; // Product does not exist, is inactive, or belongs to another seller
            }
            var updatedProduct = _mapper.Map(productDto, existingProduct);
            updatedProduct.UpdatedAt = DateTime.UtcNow;
            await _productRepository.UpdateAsync(updatedProduct);
            return true;
        }
        public async Task<bool> UpdateProductStockAsync(Guid id, UpdateProductStockDto updateProductStockDto)
        {
            var existingProduct = await _productRepository.GetByIdAsync(id);
            if (existingProduct == null || !existingProduct.IsActive)
            {
                return false; // Product does not exist or is inactive
            }
            existingProduct.Stock = updateProductStockDto.Stock;
            existingProduct.UpdatedAt = DateTime.UtcNow;
            await _productRepository.UpdateAsync(existingProduct);
            return true;

        }
        
        public async Task<IEnumerable<ProductDto>> GetProductsBySellerAsync(string sellerId)
        {
            var products = await _productRepository.GetProductsBySellerAsync(sellerId);
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }
    }
}
