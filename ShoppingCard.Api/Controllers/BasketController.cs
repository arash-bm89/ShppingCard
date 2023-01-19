using System.CodeDom.Compiler;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using System.Runtime.CompilerServices;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ShoppingCard.Api.Models;
using ShoppingCard.Domain.Interfaces;
using ShoppingCard.Domain.Models;
using ShoppingCard.Repository.Implementations;
using Basket = ShoppingCard.Domain.Models.Basket;
using BasketProduct = ShoppingCard.Domain.Models.BasketProduct;

namespace ShoppingCard.Api.Controllers
{
    // todo: ask if should i implement every crud actions of a controller
    // todo: ask if how controller property is going to use for each request. i have tested it for a number
    // and looks like it uses just one controller for every request in the application. 
    // so how requestAborted is going to work for each one right?

    /// <summary>
    /// using for actions of final basketRepository
    /// </summary>
    [ApiController]
    [Route("api/baskets")]
    public class BasketController: BaseController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IBasketProductRepository _basketProductRepository;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketRepository, IMapper mapper, IBasketProductRepository basketProductRepository)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
            _basketProductRepository = basketProductRepository;
        }

        /// <summary>
        /// get basketRepository using id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Basket>> Get(Guid id)
        {
            var basket = await _basketRepository.GetAsync(id, HttpContext.RequestAborted);

            if (basket == null)
            {
                return NotFound();
            }

            return Ok(basket);
        }


        // todo: logic is going to be changed after adding cache and user
        /// <summary>
        /// create basketRepository using BasketRequest
        /// </summary>
        /// <param name="basketRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Create(BasketRequest basketRequest)
        {
            var basket = _mapper.Map<Basket>(basketRequest);
            basket.Id = Guid.NewGuid();
            await _basketRepository.CreateAsync(basket, HttpContext.RequestAborted);
            return Ok($"basketRepository created with id: {basket.Id} and seqId: {basket.SeqId}");
        }


        /// <summary>
        /// delete basketRepository using id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBasket(Guid id)
        {
            var basket = await _basketRepository.GetAsync(id, HttpContext.RequestAborted);
            
            if (basket == null)
            {
                return NotFound();
            }

            await _basketRepository.DeleteAsync(basket, HttpContext.RequestAborted);
            return Ok();
        }


        /// <summary>
        /// getting products of a basketRepository
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:guid}/products")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<BasketProduct>>> GetBasketProducts(Guid id)
        {
            var basket = await _basketRepository.GetAsync(id, HttpContext.RequestAborted);

            if (basket == null)
                return NotFound();

            return Ok(basket);
        }


        /// <summary>
        /// get a basketProductRepository using id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:guid}/products/{productId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BasketProduct>> GetBasketProductInfo(Guid id, Guid productId)
        {
            var basketProduct = await _basketProductRepository
                .GetProductByBasketIdAsync(id, productId, HttpContext.RequestAborted);

            if (basketProduct == null)
                return NotFound();

            return Ok(basketProduct);
        }


        /// <summary>
        /// create basketProductRepository
        /// </summary>
        /// <param name="id"></param>
        /// <param name="basketProductRequest"></param>
        /// <returns></returns>
        [HttpPost("{id:guid}/products")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Create(Guid id, [FromBody] BasketProductRequest basketProductRequest)
        {
            var basketProduct = _mapper.Map<BasketProduct>(basketProductRequest);
            basketProduct.BasketId = id;
            basketProduct.Id = Guid.NewGuid();
            await _basketProductRepository.CreateAsync(basketProduct, HttpContext.RequestAborted);
            return Ok($"basketProductRepository created with id: {basketProduct.Id} and seqId: {basketProduct.SeqId}");
        }


        /// <summary>
        /// get products of a basketRepository
        /// </summary>
        /// <param name="id"></param>
        /// <param name="basketProductId"></param>
        /// <returns></returns>
        [HttpDelete("{id:guid}/products/{basketProductId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBasketProduct(Guid id, Guid basketProductId)
        {
            var basketProduct = await _basketProductRepository.GetAsync(basketProductId, HttpContext.RequestAborted);

            if (basketProduct == null)
                return NotFound();

            await _basketProductRepository.DeleteAsync(basketProduct, HttpContext.RequestAborted);
            return Ok();
        }


    }
}
