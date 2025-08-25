using AutoMapper;
using ecommerceAPI.src.EcommerceAPI.Application.DTOs;
using ecommerceAPI.src.EcommerceAPI.Domain.Entities;

namespace ecommerceAPI.src.EcommerceAPI.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryDto>()
                .ReverseMap();
            CreateMap<CreateCategoryDto, Category>()
                .ReverseMap();
            CreateMap<UpdateCategoryDto, Category>()
                .ReverseMap();

            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.CategoryDescription, opt => opt.MapFrom(src => src.Category.Description))
                .ForMember(dest => dest.SellerName, opt => opt.MapFrom(src => src.Seller.FirstName + " " + src.Seller.LastName))
                .ReverseMap()
                .ForMember(dest => dest.Category, opt => opt.Ignore());
            CreateMap<CreateProductDto, Product>()
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<UpdateProductDto, Product>()
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<UpdateProductStockDto, Product>()
                .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.Stock))
                .ReverseMap();

            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<RegisterUserDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<UpdateUserDto , User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<Order, OrderDto>()
                .ReverseMap()
                .ForMember(dest => dest.Items, opt => opt.Ignore());
            CreateMap<CreateOrderDto, Order>()
                .ForMember(dest => dest.Items, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<UpdateOrderDto, Order>()
                .ForMember(dest => dest.Items, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<OrderItem, OrderItemDto>()
                .ReverseMap();
            CreateMap<CreateOrderItemDto, OrderItem>()
                .ReverseMap();
            CreateMap<UpdateOrderItemDto, OrderItem>()
                .ReverseMap();


        }
    }
}
