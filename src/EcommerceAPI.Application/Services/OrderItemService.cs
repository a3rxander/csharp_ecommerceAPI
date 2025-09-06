using AutoMapper;
using ecommerceAPI.src.EcommerceAPI.Application.DTOs;
using ecommerceAPI.src.EcommerceAPI.Application.Interfaces;
using ecommerceAPI.src.EcommerceAPI.Domain.Entities;
using ecommerceAPI.src.EcommerceAPI.Domain.Repositories;
using System.Linq;

namespace ecommerceAPI.src.EcommerceAPI.Application.Services
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IMapper _mapper;

        public OrderItemService(IOrderRepository orderRepository, IOrderItemRepository orderItemRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _mapper = mapper;
        }

        public async Task<OrderItemDto> AddItemAsync(Guid orderId, string UserId, CreateOrderItemDto orderItemDto)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null || order.UserId != UserId)
            {
                throw new UnauthorizedAccessException();
            }

            var item = new OrderItem
            {
                Id = Guid.NewGuid(),
                OrderId = orderId,
                ProductId = orderItemDto.ProductId,
                Quantity = orderItemDto.Quantity,
                UnitPrice = orderItemDto.UnitPrice,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var createdItem = await _orderItemRepository.AddAsync(item);

            order.TotalAmount += item.UnitPrice * item.Quantity;
            order.UpdatedAt = DateTime.UtcNow;
            await _orderRepository.UpdateAsync(order);

            return _mapper.Map<OrderItemDto>(createdItem);
        }

        public async Task<bool> DeleteItemAsync(Guid id, string UserId)
        {
            var item = await _orderItemRepository.GetByIdAsync(id);
            if (item == null)
            {
                return false;
            }
            var order = await _orderRepository.GetByIdAsync(item.OrderId);
            if (order == null || order.UserId != UserId)
            {
                return false;
            }

            await _orderItemRepository.DeleteAsync(id);
            order.TotalAmount -= item.UnitPrice * item.Quantity;
            order.UpdatedAt = DateTime.UtcNow;
            await _orderRepository.UpdateAsync(order);
            return true;
        }

        public async Task<OrderItemDto?> GetItemByIdAsync(Guid id, string UserId)
        {
            var item = await _orderItemRepository.GetByIdAsync(id);
            if (item == null)
            {
                return null;
            }
            var order = await _orderRepository.GetByIdAsync(item.OrderId);
            if (order == null || order.UserId != UserId)
            {
                return null;
            }
            return _mapper.Map<OrderItemDto>(item);
        }

        public async Task<IEnumerable<OrderItemDto>> GetItemsByOrderIdAsync(Guid orderId, string UserId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null || order.UserId != UserId)
            {
                return Enumerable.Empty<OrderItemDto>();
            }
            var items = await _orderItemRepository.GetItemsByOrderIdAsync(orderId);
            return _mapper.Map<IEnumerable<OrderItemDto>>(items);
        }

        public async Task<bool> UpdateItemAsync(Guid id, string UserId, UpdateOrderItemDto orderItemDto)
        {
            var item = await _orderItemRepository.GetByIdAsync(id);
            if (item == null)
            {
                return false;
            }
            var order = await _orderRepository.GetByIdAsync(item.OrderId);
            if (order == null || order.UserId != UserId)
            {
                return false;
            }

            order.TotalAmount -= item.UnitPrice * item.Quantity;
            item.Quantity = orderItemDto.Quantity;
            item.UnitPrice = orderItemDto.UnitPrice;
            item.UpdatedAt = DateTime.UtcNow;
            await _orderItemRepository.UpdateAsync(item);
            order.TotalAmount += item.UnitPrice * item.Quantity;
            order.UpdatedAt = DateTime.UtcNow;
            await _orderRepository.UpdateAsync(order);
            return true;
        }
    }
}
