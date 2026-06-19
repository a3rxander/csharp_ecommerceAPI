using Mapster;
using ecommerceAPI.src.EcommerceAPI.Application.DTOs;
using ecommerceAPI.src.EcommerceAPI.Application.Interfaces;
using ecommerceAPI.src.EcommerceAPI.Domain.Entities;
using ecommerceAPI.src.EcommerceAPI.Domain.Repositories;

namespace ecommerceAPI.src.EcommerceAPI.Application.Services
{
    public class ShippingService : IShippingService
    {
        private readonly IShippingRepository _shippingRepository;
        private readonly IOrderRepository _orderRepository;

        public ShippingService(IShippingRepository shippingRepository, IOrderRepository orderRepository)
        {
            _shippingRepository = shippingRepository;
            _orderRepository = orderRepository;
        }

        public async Task<ShippingDto> CreateShippingAsync(CreateShippingDto shippingDto)
        {
            if (!await _orderRepository.ExistsAsync(shippingDto.OrderId))
            {
                throw new ArgumentException("Invalid OrderId");
            }

            var shipping = shippingDto.Adapt<Shipping>();
            shipping.Id = Guid.NewGuid();
            shipping.CreatedAt = DateTime.UtcNow;
            shipping.UpdatedAt = DateTime.UtcNow;
            shipping.IsActive = true;

            var createdShipping = await _shippingRepository.AddAsync(shipping);
            return createdShipping.Adapt<ShippingDto>();
        }

        public async Task<bool> DeleteShippingAsync(Guid id)
        {
            if (!await _shippingRepository.ExistsAsync(id))
            {
                return false;
            }

            await _shippingRepository.DeleteAsync(id);
            return true;
        }

        public async Task<IEnumerable<ShippingDto>> GetAllShippingsAsync()
        {
            var shippings = await _shippingRepository.GetAllAsync();
            return shippings.Adapt<IEnumerable<ShippingDto>>();
        }

        public async Task<ShippingDto?> GetShippingByIdAsync(Guid id)
        {
            var shipping = await _shippingRepository.GetByIdAsync(id);
            return shipping == null ? null : shipping.Adapt<ShippingDto>();
        }

        public async Task<bool> UpdateShippingAsync(Guid id, UpdateShippingDto shippingDto)
        {
            var existingShipping = await _shippingRepository.GetByIdAsync(id);
            if (existingShipping == null || !existingShipping.IsActive)
            {
                return false;
            }

            shippingDto.Adapt(existingShipping);
            existingShipping.UpdatedAt = DateTime.UtcNow;
            await _shippingRepository.UpdateAsync(existingShipping);
            return true;
        }
    }
}
