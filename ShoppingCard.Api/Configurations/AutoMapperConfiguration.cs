using AutoMapper;
using Common.Models;
using ShoppingCard.Api.Models;
using ShoppingCard.Domain.Dtos;
using ShoppingCard.Domain.Models;

namespace ShoppingCard.Api.Configurations;

public class AutoMapperConfiguration : Profile
{
    public AutoMapperConfiguration()
    {
        CreateMap<Order, OrderRequest>().ReverseMap();
        CreateMap<Order, OrderResponse>().ReverseMap();
        CreateMap<Product, ProductRequest>().ReverseMap();
        CreateMap<Product, ProductResponse>().ReverseMap();
        CreateMap<OrderProduct, OrderProductRequest>().ReverseMap();
        CreateMap<OrderProduct, OrderProductResponse>().ReverseMap();
        CreateMap(typeof(PaginatedResult<>), typeof(PaginatedResponseResult<>)).ReverseMap();
        CreateMap<Product, CachedProductDto>();
        CreateMap<CachedBasket, CachedBasketDto>();
        CreateMap<User, UserCreateRequest>().ReverseMap();
        CreateMap<User, UserResponse>().ReverseMap();
        CreateMap<User, UserCreateDto>().ReverseMap();
        CreateMap<UserCreateRequest, UserCreateDto>();
        CreateMap<UserLoginRequest, UserLoginDto>();
        CreateMap<UserCreateDto, UserResponse>();
    }
}