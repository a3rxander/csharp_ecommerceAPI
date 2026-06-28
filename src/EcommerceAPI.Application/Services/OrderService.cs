using Mapster;
using ecommerceAPI.src.EcommerceAPI.Application.DTOs;
using ecommerceAPI.src.EcommerceAPI.Application.Interfaces;
using ecommerceAPI.src.EcommerceAPI.Domain.Entities;
using ecommerceAPI.src.EcommerceAPI.Domain.Enums;
using ecommerceAPI.src.EcommerceAPI.Domain.Repositories;

namespace ecommerceAPI.src.EcommerceAPI.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;

        public OrderService(IOrderRepository orderRepository, ICartRepository cartRepository)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
        }

        public async Task<OrderDto> CreateOrderAsync(string UserId, CreateOrderDto orderDto)
        {
            if (string.IsNullOrWhiteSpace(orderDto.ShippingAddress))
            {
                throw new ArgumentException("Shipping address is required.");
            }

            var cart = await _cartRepository.GetByUserIdAsync(UserId);
            var cartItems = cart?.Items
                .Where(item => item.IsActive)
                .ToList() ?? new List<CartItem>();

            if (cart == null || cartItems.Count == 0)
            {
                throw new ArgumentException("Cart is empty.");
            }

            foreach (var item in cartItems)
            {
                if (item.Quantity > item.Product.Stock)
                {
                    throw new ArgumentException($"Requested quantity for {item.Product.Name} exceeds available stock.");
                }
            }

            var now = DateTime.UtcNow;
            var order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = UserId,
                OrderDate = now,
                Status = OrderStatus.Pending,
                IsActive = true,
                CreatedAt = now,
                UpdatedAt = now,
                Items = cartItems.Select(item => new OrderItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Product.Price,
                    IsActive = true,
                    CreatedAt = now,
                    UpdatedAt = now
                }).ToList(),
                Shipping = new Shipping
                {
                    Id = Guid.NewGuid(),
                    Address = orderDto.ShippingAddress.Trim(),
                    Status = "Pending",
                    IsActive = true,
                    CreatedAt = now,
                    UpdatedAt = now
                }
            };
            order.TotalAmount = order.Items.Sum(i => i.UnitPrice * i.Quantity);
            var createdOrder = await _orderRepository.AddAsync(order);
            await _cartRepository.ClearItemsAsync(cart.Id);

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
