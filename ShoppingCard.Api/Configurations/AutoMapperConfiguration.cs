using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShoppingCard.Api.Models;
using ShoppingCard.Domain.Models;

namespace ShoppingCard.Api.Configurations
{
    public class AutoMapperConfiguration: Profile
    {
        public AutoMapperConfiguration()
        {
            CreateMap<Basket, BasketRequest>().ReverseMap();
            CreateMap<Product, ProductRequest>().ReverseMap();
            CreateMap<BasketProduct, BasketProductRequest>().ReverseMap();
        }
    }
}
