using Mapster;
using ecommerceAPI.src.EcommerceAPI.Application.DTOs;
using ecommerceAPI.src.EcommerceAPI.Application.Interfaces;
using ecommerceAPI.src.EcommerceAPI.Domain.Entities;
using ecommerceAPI.src.EcommerceAPI.Domain.Repositories;

namespace ecommerceAPI.src.EcommerceAPI.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return categories.Adapt<IEnumerable<CategoryDto>>();
        }

        public async Task<CategoryDto?> GetCategoryByIdAsync(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            return category == null ? null : category.Adapt<CategoryDto>();
        }

        public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto createCategoryDto)
        {
            var category = createCategoryDto.Adapt<Category>();
            category.Id = Guid.NewGuid();
            category.CreatedAt = DateTime.UtcNow;
            category.UpdatedAt = DateTime.UtcNow;
            category.IsActive = true;
            var createdCategory = await _categoryRepository.AddAsync(category);
            return createdCategory.Adapt<CategoryDto>();
        }

        public async Task<bool> UpdateCategoryAsync(Guid id, UpdateCategoryDto updateCategoryDto)
        {
            var existingCategory = await _categoryRepository.GetByIdAsync(id);
            if (existingCategory == null || !existingCategory.IsActive)
            {
                return false;
            } 
            var updatedCategory = updateCategoryDto.Adapt(existingCategory);
            updatedCategory.UpdatedAt = DateTime.UtcNow;
            await _categoryRepository.UpdateAsync(updatedCategory);
            return true;
        }

        public async Task<bool> DeleteCategoryAsync(Guid id)
        {
            if (!await _categoryRepository.ExistsAsync(id))
            {
                return false;
            }
            await _categoryRepository.DeleteAsync(id);
            return true;
        }
    }
}

