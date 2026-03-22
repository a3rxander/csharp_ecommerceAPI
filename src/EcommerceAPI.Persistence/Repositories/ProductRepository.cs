using ecommerceAPI.src.EcommerceAPI.Domain.Entities;
using ecommerceAPI.src.EcommerceAPI.Domain.Repositories;
using ecommerceAPI.src.EcommerceAPI.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using ecommerceAPI.src.EcommerceAPI.Application.DTOs;

namespace ecommerceAPI.src.EcommerceAPI.Persistence.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly EcommerceDbContext _db;

        public ProductRepository(EcommerceDbContext context)
        {
            _db = context;
        }

        public async Task<Product> AddAsync(Product product)
        {
            await _db.Products.AddAsync(product);
            await _db.SaveChangesAsync();
            return product;
        }

        public async Task DeleteAsync(Guid id)
        {
            await _db.Products
                .Where(p => p.Id == id)
                .ExecuteDeleteAsync();
        }

        public Task<bool> ExistsAsync(Guid id)
        {
            return _db.Products
                .AnyAsync(p => p.Id == id && p.IsActive);
        }

        public async Task<(IEnumerable<Product> Products, int Total)> GetAllAsync(ProductQueryParams queryParams)
        { 

            var query =  _db.Products.Where(p => p.IsActive); 
            
            if (!string.IsNullOrWhiteSpace(queryParams.Search))
            {
                query = query.Where(p => p.Name.Contains(queryParams.Search));
            }

            if (queryParams.CategoryIds != null && queryParams.CategoryIds.Any())
            {
                query = query.Where(p => queryParams.CategoryIds.Contains(p.CategoryId));
            }

            if(queryParams.CategoryNames != null)
            {
                var categoryNames = queryParams.CategoryNames.Split(',').Select(c => c.Trim()).ToList();
                //get category ids from category names
                var categoryIds = await _db.Categories
                    .Where(c => categoryNames.Contains(c.Name))
                    .Select(c => c.Id)
                    .ToListAsync();
                query = query.Where(p => categoryIds.Contains(p.CategoryId));
            }

            if (queryParams.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= queryParams.MinPrice);
            }

            if (queryParams.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= queryParams.MaxPrice);
            }

            var total = await query.CountAsync();   


            var products = await query 
                .Include(p => p.Category) // Eager loading of Category
                .Include(p => p.Seller) // Eager loading of Seller
                .Include(p => p.ProductImages.Where(pi => pi.IsPrimary)) // Eager loading of primary ProductImage
                .OrderBy(p => p.Name)
                .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
                .Take(queryParams.PageSize) 
                .ToListAsync();

            return (products, total);
        }

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            var product = await _db.Products
                .Where(p => p.Id == id && p.IsActive)
                .Include(p => p.Category) // Eager loading of Category
                .Include(p => p.Seller) // Eager loading of Seller
                .Include(p => p.ProductImages.Where(pi => pi.IsPrimary)) // Eager loading of primary ProductImage
                .FirstOrDefaultAsync(); 

            return product; 
            
        }

        public async Task UpdateAsync(Product product)
        {
            product.UpdatedAt = DateTime.UtcNow;
            _db.Products.Update(product);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(Guid categoryId)
        {
            return await _db.Products
                .Where(p => p.CategoryId == categoryId && p.IsActive)
                .Include(p => p.Seller) // Eager loading of Seller
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsBySellerAsync(string sellerId)
        {
            return await _db.Products
                .Where(p => p.SellerId == sellerId && p.IsActive)
                .Include(p => p.Category) // Eager loading of Category
                .OrderBy(p => p.Name)
                .ToListAsync();
        }
    }
}
