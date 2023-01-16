using System.Runtime.CompilerServices;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingCard.Api.Models;
using ShoppingCard.Domain.Interfaces;
using ShoppingCard.Domain.Models;
using ShoppingCard.Repository.Implementations;

namespace ShoppingCard.Api.Controllers
{
    [ApiController]
    [Route("api/")]
    public class BasketController: BaseController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketRepository, IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<List<Basket>> GetAll()
        {
            return await _basketRepository.GetAllAsync(RequestAbborted);
        }

        [HttpGet("{id:guid}")]
        public async Task<Basket> Get(Guid id)
        {
            return await _basketRepository.GetByIdAsync(id, RequestAbborted);
        }

        [HttpPost]
        public async Task<IActionResult> Create(BasketRequest basketRequest)
        {
            var basket = _mapper.Map<Basket>(basketRequest);
            basket.Id = Guid.NewGuid();
            await _basketRepository.CreateAsync(basket, RequestAbborted);
            return Ok($"basket created with id: {basket.Id} and seqId: {basket.SeqId}");
        }
    }
}
