using ecommerceAPI.src.EcommerceAPI.Application.DTOs;
using ecommerceAPI.src.EcommerceAPI.Application.Interfaces;
using ecommerceAPI.src.EcommerceAPI.Domain.Entities;
using ecommerceAPI.src.EcommerceAPI.Domain.Repositories;

namespace ecommerceAPI.src.EcommerceAPI.Application.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;

        public CartService(ICartRepository cartRepository, IProductRepository productRepository, IUserRepository userRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
        }

        public async Task<CartDto> AddItemAsync(string userId, AddCartItemDto itemDto)
        {
            if (itemDto.Quantity < 1)
            {
                throw new ArgumentException("Quantity must be at least 1.");
            }

            var product = await _productRepository.GetByIdAsync(itemDto.ProductId);
            if (product == null)
            {
                throw new ArgumentException("Invalid ProductId.");
            }

            var cart = await GetOrCreateCartEntityAsync(userId);
            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == itemDto.ProductId);
            var newQuantity = itemDto.Quantity + (existingItem?.Quantity ?? 0);

            if (newQuantity > product.Stock)
            {
                throw new ArgumentException("Requested quantity exceeds available stock.");
            }

            if (existingItem == null)
            {
                cart.Items.Add(new CartItem
                {
                    Id = Guid.NewGuid(),
                    CartId = cart.Id,
                    ProductId = itemDto.ProductId,
                    Product = product,
                    Quantity = itemDto.Quantity,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
            }
            else
            {
                existingItem.Quantity = newQuantity;
                existingItem.UpdatedAt = DateTime.UtcNow;
            }

            cart.UpdatedAt = DateTime.UtcNow;
            await _cartRepository.UpdateAsync(cart);

            var updatedCart = await _cartRepository.GetByUserIdAsync(userId);
            return MapCart(updatedCart!);
        }

        public async Task<bool> ClearCartAsync(string userId)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);
            if (cart == null)
            {
                return false;
            }

            await _cartRepository.ClearItemsAsync(cart.Id);
            return true;
        }

        public async Task<CartDto> CreateCartAsync(string userId)
        {
            var existingCart = await _cartRepository.GetByUserIdAsync(userId);
            if (existingCart != null)
            {
                return MapCart(existingCart);
            }

            if (!Guid.TryParse(userId, out var parsedUserId) || !await _userRepository.ExistsAsync(parsedUserId))
            {
                throw new ArgumentException("Invalid UserId.");
            }

            var cart = new Cart
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var createdCart = await _cartRepository.AddAsync(cart);
            return MapCart(createdCart);
        }

        public async Task<bool> DeleteCartAsync(string userId)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);
            if (cart == null)
            {
                return false;
            }

            await _cartRepository.DeleteAsync(cart.Id);
            return true;
        }

        public async Task<IEnumerable<CartDto>> GetAllCartsAsync()
        {
            var carts = await _cartRepository.GetAllAsync();
            return carts.Select(MapCart);
        }

        public async Task<CartDto?> GetCartByIdAsync(Guid id)
        {
            var cart = await _cartRepository.GetByIdAsync(id);
            return cart == null ? null : MapCart(cart);
        }

        public async Task<CartDto?> GetCartByUserAsync(string userId)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);
            return cart == null ? null : MapCart(cart);
        }

        public async Task<bool> RemoveItemAsync(string userId, Guid itemId)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);
            if (cart == null || !cart.Items.Any(i => i.Id == itemId))
            {
                return false;
            }

            await _cartRepository.DeleteItemAsync(itemId);
            return true;
        }

        public async Task<bool> UpdateItemAsync(string userId, Guid itemId, UpdateCartItemDto itemDto)
        {
            if (itemDto.Quantity < 1)
            {
                throw new ArgumentException("Quantity must be at least 1.");
            }

            var cart = await _cartRepository.GetByUserIdAsync(userId);
            var item = cart?.Items.FirstOrDefault(i => i.Id == itemId);
            if (cart == null || item == null)
            {
                return false;
            }

            if (itemDto.Quantity > item.Product.Stock)
            {
                throw new ArgumentException("Requested quantity exceeds available stock.");
            }

            item.Quantity = itemDto.Quantity;
            item.UpdatedAt = DateTime.UtcNow;
            cart.UpdatedAt = DateTime.UtcNow;

            await _cartRepository.UpdateAsync(cart);
            return true;
        }

        private async Task<Cart> GetOrCreateCartEntityAsync(string userId)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);
            if (cart != null)
            {
                return cart;
            }

            if (!Guid.TryParse(userId, out var parsedUserId) || !await _userRepository.ExistsAsync(parsedUserId))
            {
                throw new ArgumentException("Invalid UserId.");
            }

            return await _cartRepository.AddAsync(new Cart
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
        }

        private static CartDto MapCart(Cart cart)
        {
            var items = cart.Items
                .Where(i => i.IsActive)
                .Select(MapCartItem)
                .ToList();

            return new CartDto
            {
                Id = cart.Id,
                UserId = cart.UserId,
                Items = items,
                TotalAmount = items.Sum(i => i.TotalPrice),
                CreatedAt = cart.CreatedAt,
                UpdatedAt = cart.UpdatedAt
            };
        }

        private static CartItemDto MapCartItem(CartItem item)
        {
            var primaryImage = item.Product.ProductImages.FirstOrDefault(pi => pi.IsPrimary);

            return new CartItemDto
            {
                Id = item.Id,
                ProductId = item.ProductId,
                ProductName = item.Product.Name,
                ImageUrl = primaryImage?.ImageUrl ?? item.Product.ImageUrl,
                UnitPrice = item.Product.Price,
                Quantity = item.Quantity,
                TotalPrice = item.Product.Price * item.Quantity
            };
        }
    }
}
