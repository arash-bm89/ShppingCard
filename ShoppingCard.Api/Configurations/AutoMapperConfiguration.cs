using AutoMapper;
using ShoppingCard.Api.Models;
using ShoppingCard.Domain.Models;

namespace ShoppingCard.Api.Configurations
{
    public class AutoMapperConfiguration: Profile
    {
        public AutoMapperConfiguration()
        {
            CreateMap<Basket, BasketRequest>().ReverseMap();
        }
    }
}
