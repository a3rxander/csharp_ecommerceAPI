using ecommerceAPI.src.EcommerceAPI.Application.DTOs;

namespace ecommerceAPI.src.EcommerceAPI.Application.Interfaces
{
    public interface IStockMovementService
    {
        Task<IEnumerable<StockMovementDto>> GetAllStockMovementsAsync();
        Task<StockMovementDto?> GetStockMovementByIdAsync(Guid id);
        Task<StockMovementDto> CreateStockMovementAsync(CreateStockMovementDto stockMovementDto);
        Task<bool> UpdateStockMovementAsync(Guid id, UpdateStockMovementDto stockMovementDto);
        Task<bool> DeleteStockMovementAsync(Guid id);
        Task<IEnumerable<StockMovementDto>> GetStockMovementsByProductAsync(Guid productId);
    }
}
