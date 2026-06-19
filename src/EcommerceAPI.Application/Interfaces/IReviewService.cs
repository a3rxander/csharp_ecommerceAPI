using ecommerceAPI.src.EcommerceAPI.Application.DTOs;

namespace ecommerceAPI.src.EcommerceAPI.Application.Interfaces
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewDto>> GetAllReviewsAsync();
        Task<ReviewDto?> GetReviewByIdAsync(Guid id);
        Task<ReviewDto> CreateReviewAsync(CreateReviewDto reviewDto);
        Task<bool> UpdateReviewAsync(Guid id, UpdateReviewDto reviewDto);
        Task<bool> DeleteReviewAsync(Guid id);
        Task<IEnumerable<ReviewDto>> GetReviewsByProductAsync(Guid productId);
        Task<IEnumerable<ReviewDto>> GetReviewsByUserAsync(string userId);
    }
}
