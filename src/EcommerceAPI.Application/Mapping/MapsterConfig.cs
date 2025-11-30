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
                .Map(dest => dest.CategoryName, src => src.Category == null ? string.Empty : src.Category.Name)
                .Map(dest => dest.CategoryDescription,
                    src => src.Category == null ? string.Empty : src.Category.Description)
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
        }
    }
}

