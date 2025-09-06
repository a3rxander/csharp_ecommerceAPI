using AutoMapper;
using ecommerceAPI.src.EcommerceAPI.Application.DTOs;
using ecommerceAPI.src.EcommerceAPI.Application.Interfaces;
using ecommerceAPI.src.EcommerceAPI.Domain.Entities;
using ecommerceAPI.src.EcommerceAPI.Domain.Repositories;
using System.Linq;

namespace ecommerceAPI.src.EcommerceAPI.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<OrderDto> CreateOrderAsync(string UserId, CreateOrderDto orderDto)
        {
            var order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = UserId,
                OrderDate = DateTime.UtcNow,
                Status = "Pending",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Items = orderDto.Items.Select(item => new OrderItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }).ToList()
            };
            order.TotalAmount = order.Items.Sum(i => i.UnitPrice * i.Quantity);
            var createdOrder = await _orderRepository.AddAsync(order);
            return _mapper.Map<OrderDto>(createdOrder);
        }

        public async Task<bool> DeleteOrderAsync(Guid id, string UserId)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null || order.UserId != UserId)
            {
                return false;
            }
            await _orderRepository.DeleteAsync(id);
            return true;
        }

        public async Task<OrderDto?> GetOrderByIdAsync(Guid id, string UserId)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null || order.UserId != UserId)
            {
                return null;
            }
            return _mapper.Map<OrderDto>(order);
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersByUserAsync(string UserId)
        {
            var orders = await _orderRepository.GetOrdersByUserAsync(UserId);
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<bool> UpdateOrderAsync(Guid id, string UserId, UpdateOrderDto orderDto)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null || order.UserId != UserId)
            {
                return false;
            }
            order.Status = orderDto.Status;
            order.UpdatedAt = DateTime.UtcNow;
            await _orderRepository.UpdateAsync(order);
            return true;
        }
    }
}
