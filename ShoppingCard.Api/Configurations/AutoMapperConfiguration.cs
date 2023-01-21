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
        CreateMap<Basket, BasketRequest>().ReverseMap();
        CreateMap<Basket, BasketResponse>().ReverseMap();
        CreateMap<Product, ProductRequest>().ReverseMap();
        CreateMap<Product, ProductResponse>().ReverseMap();
        CreateMap<BasketProduct, BasketProductRequest>().ReverseMap();
        CreateMap<BasketProduct, BasketProductResponse>().ReverseMap();
        CreateMap(typeof(PaginatedResult<>), typeof(PaginatedResponseResult<>)).ReverseMap();
        CreateMap<Product, CachedProductDto>();
        CreateMap<CachedBasket, CachedBasketDto>();
    }
}