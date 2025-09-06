using Mapster;
using ecommerceAPI.src.EcommerceAPI.Application.DTOs;
using ecommerceAPI.src.EcommerceAPI.Application.Interfaces;
using ecommerceAPI.src.EcommerceAPI.Domain.Entities;
using ecommerceAPI.src.EcommerceAPI.Domain.Repositories;

namespace ecommerceAPI.src.EcommerceAPI.Application.Services
{
    public class ProductImageService : IProductImageService
    {
        private readonly IProductImageRepository _productImageRepository;

        public ProductImageService(IProductImageRepository productImageRepository)
        {
            _productImageRepository = productImageRepository;
        }

        public async Task<IEnumerable<ProductImageDto>> GetImagesByProductIdAsync(Guid productId)
        {
            var images = await _productImageRepository.GetImagesByProductIdAsync(productId);
            return images.Adapt<IEnumerable<ProductImageDto>>();
        }

        public async Task<ProductImageDto?> GetByIdAsync(Guid id)
        {
            var image = await _productImageRepository.GetByIdAsync(id);
            return image == null ? null : image.Adapt<ProductImageDto>();
        }

        public async Task<ProductImageDto> CreateProductImageAsync(CreateProductImageDto createProductImageDto)
        {
            var ImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            if (!Directory.Exists(ImagePath))
            {
                Directory.CreateDirectory(ImagePath);
            }
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(createProductImageDto.ImageFile.FileName)}";
            var filePath = Path.Combine(ImagePath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await createProductImageDto.ImageFile.CopyToAsync(stream);
            }
            
            var newimage = new ProductImage
            {
                ProductId = createProductImageDto.ProductId,
                ImageUrl = $"/images/{fileName}",
                IsPrimary = createProductImageDto.IsPrimary,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };
            var addedImage = await _productImageRepository.AddAsync(newimage);
            return addedImage.Adapt<ProductImageDto>();
        }
        public async Task UpdateProductImageAsync(Guid id, UpdateProductImageDto updateProductImageDto)
        {
            var existingImage = await _productImageRepository.GetByIdAsync(id);
            if (existingImage == null)
            {
                throw new KeyNotFoundException("Product image not found.");
            }

            updateProductImageDto.Adapt(existingImage);
            await _productImageRepository.UpdateAsync(existingImage);
        }
        public async Task DeleteProductImageAsync(Guid id, string userId)
        {
            var existingImage = await _productImageRepository.GetByIdAsync(id);
            if (existingImage == null)
            {
                throw new KeyNotFoundException("Product image not found.");
            }

            if(existingImage.Product.SellerId != userId)
            {
                throw new UnauthorizedAccessException("You do not have permission to delete this image.");
            }

            await _productImageRepository.DeleteAsync(id);
        }
    }
}