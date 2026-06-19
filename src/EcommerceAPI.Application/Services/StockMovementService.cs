using Mapster;
using ecommerceAPI.src.EcommerceAPI.Application.DTOs;
using ecommerceAPI.src.EcommerceAPI.Application.Interfaces;
using ecommerceAPI.src.EcommerceAPI.Domain.Entities;
using ecommerceAPI.src.EcommerceAPI.Domain.Repositories;

namespace ecommerceAPI.src.EcommerceAPI.Application.Services
{
    public class StockMovementService : IStockMovementService
    {
        private readonly IStockMovementRepository _stockMovementRepository;
        private readonly IProductRepository _productRepository;

        public StockMovementService(IStockMovementRepository stockMovementRepository, IProductRepository productRepository)
        {
            _stockMovementRepository = stockMovementRepository;
            _productRepository = productRepository;
        }

        public async Task<StockMovementDto> CreateStockMovementAsync(CreateStockMovementDto stockMovementDto)
        {
            if (!await _productRepository.ExistsAsync(stockMovementDto.ProductId))
            {
                throw new ArgumentException("Invalid ProductId");
            }

            var stockMovement = stockMovementDto.Adapt<StockMovement>();
            stockMovement.Id = Guid.NewGuid();
            stockMovement.MovementDate = DateTime.UtcNow;
            stockMovement.CreatedAt = DateTime.UtcNow;
            stockMovement.UpdatedAt = DateTime.UtcNow;
            stockMovement.IsActive = true;

            var createdStockMovement = await _stockMovementRepository.AddAsync(stockMovement);
            return createdStockMovement.Adapt<StockMovementDto>();
        }

        public async Task<bool> DeleteStockMovementAsync(Guid id)
        {
            if (!await _stockMovementRepository.ExistsAsync(id))
            {
                return false;
            }

            await _stockMovementRepository.DeleteAsync(id);
            return true;
        }

        public async Task<IEnumerable<StockMovementDto>> GetAllStockMovementsAsync()
        {
            var stockMovements = await _stockMovementRepository.GetAllAsync();
            return stockMovements.Adapt<IEnumerable<StockMovementDto>>();
        }

        public async Task<StockMovementDto?> GetStockMovementByIdAsync(Guid id)
        {
            var stockMovement = await _stockMovementRepository.GetByIdAsync(id);
            return stockMovement == null ? null : stockMovement.Adapt<StockMovementDto>();
        }

        public async Task<IEnumerable<StockMovementDto>> GetStockMovementsByProductAsync(Guid productId)
        {
            var stockMovements = await _stockMovementRepository.GetStockMovementsByProductAsync(productId);
            return stockMovements.Adapt<IEnumerable<StockMovementDto>>();
        }

        public async Task<bool> UpdateStockMovementAsync(Guid id, UpdateStockMovementDto stockMovementDto)
        {
            var existingStockMovement = await _stockMovementRepository.GetByIdAsync(id);
            if (existingStockMovement == null || !existingStockMovement.IsActive)
            {
                return false;
            }

            stockMovementDto.Adapt(existingStockMovement);
            existingStockMovement.UpdatedAt = DateTime.UtcNow;
            await _stockMovementRepository.UpdateAsync(existingStockMovement);
            return true;
        }
    }
}
