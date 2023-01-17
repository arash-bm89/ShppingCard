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

namespace ShoppingCard.Api.Controllers
{
    // todo: ask if should i implement every crud actions of a controller
    // todo: ask if how controller property is going to use for each request. i have tested it for a number
    // and looks like it uses just one controller for every request in the application. 
    // so how requestAborted is going to work for each one right?

    /// <summary>
    /// using for actions of final basket
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


        // todo: going to be commented after finalize
        /// <summary>
        /// Get all the baskets (for testing)
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<List<Basket>?> GetAll()
        {
            return await _basketRepository.GetAllAsync(HttpContext.RequestAborted);
        }


        /// <summary>
        /// get basket using id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Basket>> Get(Guid id)
        {
            var basket = await _basketRepository.GetByIdAsync(id, HttpContext.RequestAborted);

            if (basket == null)
            {
                return NotFound();
            }

            return Ok(basket);
        }


        // todo: logic is going to be changed after adding cache and user
        /// <summary>
        /// create basket using BasketRequest
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
            return Ok($"basket created with id: {basket.Id} and seqId: {basket.SeqId}");
        }


        /// <summary>
        /// delete basket using id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBasket(Guid id)
        {
            var basket = await _basketRepository.GetByIdAsync(id, HttpContext.RequestAborted);
            
            if (basket == null)
            {
                return NotFound();
            }

            await _basketRepository.DeleteAsync(basket, HttpContext.RequestAborted);
            return Ok();
        }


        /// <summary>
        /// getting products of a basket
        /// </summary>
        /// <param name="basketId"></param>
        /// <returns></returns>
        [HttpGet("{basketId:guid}/products")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<BasketProduct>>> GetBasketProducts(Guid basketId)
        {
            var basket = await _basketRepository.GetByIdAsync(basketId, HttpContext.RequestAborted);

            if (basket == null)
                return NotFound();

            return Ok(basket);
        }


        /// <summary>
        /// get a basketProduct using id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{basketId:guid}/products/{productId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BasketProduct>> GetBasketProductInfo(Guid basketId, Guid productId)
        {
            var basketProduct = await _basketProductRepository
                .GetProductByBasketIdAsync(basketId, productId, HttpContext.RequestAborted);

            if (basketProduct == null)
                return NotFound();

            return Ok(basketProduct);
        }


        /// <summary>
        /// create basketProduct
        /// </summary>
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
            return Ok($"basketProduct created with id: {basketProduct.Id} and seqId: {basketProduct.SeqId}");
        }

        /// <summary>
        /// get products of a basket
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{basketId:guid}/products/{basketProductId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBasketProduct(Guid basktId, Guid basketProductId)
        {
            var basketProduct = await _basketProductRepository.GetByIdAsync(basketProductId, HttpContext.RequestAborted);

            if (basketProduct == null)
                return NotFound();

            await _basketProductRepository.DeleteAsync(basketProduct, HttpContext.RequestAborted);
            return Ok();
        }

    }
}
