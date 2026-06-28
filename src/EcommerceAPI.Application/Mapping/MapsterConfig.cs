using Mapster;
using ecommerceAPI.src.EcommerceAPI.Application.DTOs;
using ecommerceAPI.src.EcommerceAPI.Domain.Entities;

namespace ecommerceAPI.src.EcommerceAPI.Application.Mapping
{
    public static class MapsterConfig
    {
        public static void RegisterMappings()
        {
            // Category <-> CategoryDto
            TypeAdapterConfig<Category, CategoryDto>.NewConfig().TwoWays();
            TypeAdapterConfig<CreateCategoryDto, Category>.NewConfig().TwoWays();
            TypeAdapterConfig<UpdateCategoryDto, Category>.NewConfig().TwoWays();

            // Product mappings
            TypeAdapterConfig<Product, ProductDto>.NewConfig()
                .Map(dest => dest.Category, 
                src => src.Category)
                .Map(dest => dest.PrimaryImage,
                 src => src.ProductImages.FirstOrDefault(pi => pi.IsPrimary))
                .Map(dest => dest.SellerName,
                    src => src.Seller != null ? $"{src.Seller.FirstName} {src.Seller.LastName}".Trim() : string.Empty)
                .TwoWays();

            TypeAdapterConfig<CreateProductDto, Product>.NewConfig()
                .Ignore(dest => dest.Category)
                .TwoWays();

            TypeAdapterConfig<UpdateProductDto, Product>.NewConfig()
                .Ignore(dest => dest.Category)
                .TwoWays();

            TypeAdapterConfig<UpdateProductStockDto, Product>.NewConfig()
                .Map(dest => dest.Stock, src => src.Stock)
                .TwoWays();

            // User mappings
            TypeAdapterConfig<User, UserDto>.NewConfig()
                .Map(dest => dest.Username, src => src.UserName)
                .TwoWays();

            TypeAdapterConfig<RegisterUserDto, User>.NewConfig()
                .Map(dest => dest.UserName, src => src.Username)
                .Ignore(dest => dest.PasswordHash!)
                .TwoWays();

            TypeAdapterConfig<UpdateUserDto, User>.NewConfig()
                .Ignore(dest => dest.PasswordHash!)
                .TwoWays();

            // Order mappings
            TypeAdapterConfig<Order, OrderDto>.NewConfig().TwoWays();
            TypeAdapterConfig<CreateOrderDto, Order>.NewConfig()
                .Ignore(dest => dest.Items)
                .TwoWays();
            TypeAdapterConfig<UpdateOrderDto, Order>.NewConfig()
                .Ignore(dest => dest.Items)
                .TwoWays();

            // OrderItem mappings
            TypeAdapterConfig<OrderItem, OrderItemDto>.NewConfig().TwoWays();
            TypeAdapterConfig<CreateOrderItemDto, OrderItem>.NewConfig().TwoWays();
            TypeAdapterConfig<UpdateOrderItemDto, OrderItem>.NewConfig().TwoWays();

            // ProductImage mappings
            TypeAdapterConfig<ProductImage, ProductImageDto>.NewConfig().TwoWays();
            TypeAdapterConfig<CreateProductImageDto, ProductImage>.NewConfig()
                .Ignore(dest => dest.Product)
                .TwoWays();
            TypeAdapterConfig<UpdateProductImageDto, ProductImage>.NewConfig()
                .Ignore(dest => dest.Product)
                .TwoWays();

            // Payment mappings
            TypeAdapterConfig<Payment, PaymentDto>.NewConfig().TwoWays();
            TypeAdapterConfig<CreatePaymentDto, Payment>.NewConfig()
                .Ignore(dest => dest.Order)
                .TwoWays();
            TypeAdapterConfig<UpdatePaymentDto, Payment>.NewConfig()
                .Ignore(dest => dest.Order)
                .TwoWays();

            // Shipping mappings
            TypeAdapterConfig<Shipping, ShippingDto>.NewConfig().TwoWays();
            TypeAdapterConfig<CreateShippingDto, Shipping>.NewConfig()
                .Ignore(dest => dest.Order)
                .TwoWays();
            TypeAdapterConfig<UpdateShippingDto, Shipping>.NewConfig()
                .Ignore(dest => dest.Order)
                .TwoWays();

            // Review mappings
            TypeAdapterConfig<Review, ReviewDto>.NewConfig()
                .Map(dest => dest.ReviewerName,
                    src => src.User != null
                        ? (!string.IsNullOrWhiteSpace($"{src.User.FirstName} {src.User.LastName}".Trim())
                            ? $"{src.User.FirstName} {src.User.LastName}".Trim()
                            : src.User.UserName ?? string.Empty)
                        : string.Empty);
            TypeAdapterConfig<CreateReviewDto, Review>.NewConfig()
                .Ignore(dest => dest.Product)
                .Ignore(dest => dest.User)
                .TwoWays();
            TypeAdapterConfig<UpdateReviewDto, Review>.NewConfig()
                .Ignore(dest => dest.Product)
                .Ignore(dest => dest.User)
                .TwoWays();

            // StockMovement mappings
            TypeAdapterConfig<StockMovement, StockMovementDto>.NewConfig().TwoWays();
            TypeAdapterConfig<CreateStockMovementDto, StockMovement>.NewConfig()
                .Ignore(dest => dest.Product)
                .TwoWays();
            TypeAdapterConfig<UpdateStockMovementDto, StockMovement>.NewConfig()
                .Ignore(dest => dest.Product)
                .TwoWays();

            // Cart mappings
            TypeAdapterConfig<Cart, CartDto>.NewConfig().TwoWays();
            TypeAdapterConfig<CartItem, CartItemDto>.NewConfig().TwoWays();
            TypeAdapterConfig<AddCartItemDto, CartItem>.NewConfig()
                .Ignore(dest => dest.Cart)
                .Ignore(dest => dest.Product)
                .TwoWays();
            TypeAdapterConfig<UpdateCartItemDto, CartItem>.NewConfig()
                .Ignore(dest => dest.Cart)
                .Ignore(dest => dest.Product)
                .TwoWays();
        }
    }
}

