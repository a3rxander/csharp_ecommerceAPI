using Mapster;
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

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
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
            return createdOrder.Adapt<OrderDto>();
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
            return order.Adapt<OrderDto>();
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersByUserAsync(string UserId)
        {
            var orders = await _orderRepository.GetOrdersByUserAsync(UserId);
            return orders.Adapt<IEnumerable<OrderDto>>();
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
