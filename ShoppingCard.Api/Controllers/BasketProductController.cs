using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShoppingCard.Api.Models;
using ShoppingCard.Domain.Interfaces;
using ShoppingCard.Domain.Models;

namespace ShoppingCard.Api.Controllers
{
    /// <summary>
    /// actions of basketProducts 
    /// </summary>
    [Route("api/basketProducts")]
    public class BasketProductController: BaseController
    {
        private readonly IBasketProductRepository _basketProductRepository;
        private readonly IMapper _mapper;
        public BasketProductController(IBasketProductRepository basketProductRepository, IMapper mapper)
        {
            _basketProductRepository = basketProductRepository;
            _mapper = mapper;
        }


        // todo: going to be commented after finalize
        /// <summary>
        /// Get all the baskets (for testing)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<BasketProduct>?> GetAll()
        {
            return await _basketProductRepository.GetAllAsync(HttpContext.RequestAborted);
        }


    }
}
