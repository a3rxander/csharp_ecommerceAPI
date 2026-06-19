using Mapster;
using ecommerceAPI.src.EcommerceAPI.Application.DTOs;
using ecommerceAPI.src.EcommerceAPI.Application.Interfaces;
using ecommerceAPI.src.EcommerceAPI.Domain.Entities;
using ecommerceAPI.src.EcommerceAPI.Domain.Repositories;

namespace ecommerceAPI.src.EcommerceAPI.Application.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;

        public ReviewService(IReviewRepository reviewRepository, IProductRepository productRepository, IUserRepository userRepository)
        {
            _reviewRepository = reviewRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
        }

        public async Task<ReviewDto> CreateReviewAsync(CreateReviewDto reviewDto)
        {
            if (!await _productRepository.ExistsAsync(reviewDto.ProductId))
            {
                throw new ArgumentException("Invalid ProductId");
            }

            if (!Guid.TryParse(reviewDto.UserId, out var userId) || !await _userRepository.ExistsAsync(userId))
            {
                throw new ArgumentException("Invalid UserId");
            }

            var review = reviewDto.Adapt<Review>();
            review.Id = Guid.NewGuid();
            review.ReviewDate = DateTime.UtcNow;
            review.CreatedAt = DateTime.UtcNow;
            review.UpdatedAt = DateTime.UtcNow;
            review.IsActive = true;

            var createdReview = await _reviewRepository.AddAsync(review);
            return createdReview.Adapt<ReviewDto>();
        }

        public async Task<bool> DeleteReviewAsync(Guid id)
        {
            if (!await _reviewRepository.ExistsAsync(id))
            {
                return false;
            }

            await _reviewRepository.DeleteAsync(id);
            return true;
        }

        public async Task<IEnumerable<ReviewDto>> GetAllReviewsAsync()
        {
            var reviews = await _reviewRepository.GetAllAsync();
            return reviews.Adapt<IEnumerable<ReviewDto>>();
        }

        public async Task<ReviewDto?> GetReviewByIdAsync(Guid id)
        {
            var review = await _reviewRepository.GetByIdAsync(id);
            return review == null ? null : review.Adapt<ReviewDto>();
        }

        public async Task<IEnumerable<ReviewDto>> GetReviewsByProductAsync(Guid productId)
        {
            var reviews = await _reviewRepository.GetReviewsByProductAsync(productId);
            return reviews.Adapt<IEnumerable<ReviewDto>>();
        }

        public async Task<IEnumerable<ReviewDto>> GetReviewsByUserAsync(string userId)
        {
            var reviews = await _reviewRepository.GetReviewsByUserAsync(userId);
            return reviews.Adapt<IEnumerable<ReviewDto>>();
        }

        public async Task<bool> UpdateReviewAsync(Guid id, UpdateReviewDto reviewDto)
        {
            var existingReview = await _reviewRepository.GetByIdAsync(id);
            if (existingReview == null || !existingReview.IsActive)
            {
                return false;
            }

            reviewDto.Adapt(existingReview);
            existingReview.UpdatedAt = DateTime.UtcNow;
            await _reviewRepository.UpdateAsync(existingReview);
            return true;
        }
    }
}
