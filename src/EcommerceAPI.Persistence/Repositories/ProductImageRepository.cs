using ecommerceAPI.src.EcommerceAPI.Domain.Entities;
using ecommerceAPI.src.EcommerceAPI.Domain.Repositories;
using ecommerceAPI.src.EcommerceAPI.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace ecommerceAPI.src.EcommerceAPI.Persistence.Repositories
{
    public class ProductImageRepository : IProductImageRepository
    {
        private readonly EcommerceDbContext _db;

        public ProductImageRepository(EcommerceDbContext context)
        {
            _db = context;
        }


        public async Task<IEnumerable<ProductImage>> GetImagesByProductIdAsync(Guid productId)
        {
            return await _db.ProductImages
                .Where(pi => pi.ProductId == productId && pi.IsActive)
                .ToListAsync();
        }

        public async Task<ProductImage?> GetByIdAsync(Guid id)
        {
            return await _db.ProductImages
                .Where(pi => pi.Id == id && pi.IsActive)
                .FirstOrDefaultAsync();
        }

        public async Task<ProductImage> AddAsync(ProductImage productImage)
        {
            await _db.ProductImages.AddAsync(productImage);
            await _db.SaveChangesAsync();
            return productImage;
        }

        public async Task DeleteAsync(Guid id)
        {
            await _db.ProductImages
                .Where(pi => pi.Id == id)
                .ExecuteDeleteAsync();
        }  

        public async Task UpdateAsync(ProductImage productImage)
        {
            _db.ProductImages.Update(productImage);
            await _db.SaveChangesAsync();
        }
    }
}