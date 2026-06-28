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
            reviewDto.Comment ??= string.Empty;
            ValidateReview(reviewDto.Rating, reviewDto.Comment);

            if (reviewDto.ProductId == Guid.Empty)
            {
                throw new ArgumentException("ProductId is required.");
            }

            if (!Guid.TryParse(reviewDto.UserId, out var userId))
            {
                throw new ArgumentException("Invalid UserId");
            }

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("Invalid UserId");
            }

            var product = await _productRepository.GetByIdAsync(reviewDto.ProductId);
            if (product == null)
            {
                throw new ArgumentException("Invalid ProductId");
            }

            if (product.SellerId == reviewDto.UserId)
            {
                throw new UnauthorizedAccessException("Product owners cannot review their own products.");
            }

            var review = reviewDto.Adapt<Review>();
            review.Id = Guid.NewGuid();
            review.ReviewDate = DateTime.UtcNow;
            review.CreatedAt = DateTime.UtcNow;
            review.UpdatedAt = DateTime.UtcNow;
            review.IsActive = true;

            var createdReview = await _reviewRepository.AddAsync(review);
            var createdReviewDto = createdReview.Adapt<ReviewDto>();
            createdReviewDto.ReviewerName = GetUserDisplayName(user);
            return createdReviewDto;
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
            reviewDto.Comment ??= string.Empty;
            ValidateReview(reviewDto.Rating, reviewDto.Comment);

            var existingReview = await _reviewRepository.GetByIdAsync(id);
            if (existingReview == null || !existingReview.IsActive)
            {
                return false;
            }

            existingReview.Rating = reviewDto.Rating;
            existingReview.Comment = reviewDto.Comment;
            existingReview.UpdatedAt = DateTime.UtcNow;
            await _reviewRepository.UpdateAsync(existingReview);
            return true;
        }

        private static void ValidateReview(int rating, string comment)
        {
            if (rating < 1 || rating > 5)
            {
                throw new ArgumentException("Rating must be between 1 and 5.");
            }

            comment ??= string.Empty;

            if (comment.Length > 1000)
            {
                throw new ArgumentException("Comment cannot exceed 1000 characters.");
            }
        }

        private static string GetUserDisplayName(User user)
        {
            var fullName = $"{user.FirstName} {user.LastName}".Trim();
            return string.IsNullOrWhiteSpace(fullName) ? user.UserName ?? string.Empty : fullName;
        }
    }
}
