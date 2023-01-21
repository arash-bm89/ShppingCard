using AutoMapper;
using Common.Models;
using Microsoft.AspNetCore.Mvc;
using ShoppingCard.Api.Models;
using ShoppingCard.Domain.Filters;
using ShoppingCard.Domain.Interfaces;
using ShoppingCard.Domain.Models;
using ShoppingCard.Service.IServices;

namespace ShoppingCard.Api.Controllers;

/// <summary>
///     using for actions of final basketRepository
/// </summary>
[ApiController]
[Route("baskets")]
public class BasketController : BaseController
{
    private readonly IBasketProductRepository _basketProductRepository;
    private readonly IBasketRepository _basketRepository;
    private readonly ICachedBasketService _cachingService;
    private readonly IMapper _mapper;
    private readonly IProductRepository _productRepository;

    public BasketController(IBasketRepository basketRepository,
        IMapper mapper,
        IBasketProductRepository basketProductRepository,
        ICachedBasketService cachingService,
        IProductRepository productRepository)
    {
        _basketRepository = basketRepository;
        _mapper = mapper;
        _basketProductRepository = basketProductRepository;
        _cachingService = cachingService;
        _productRepository = productRepository;
    }


    /// <summary>
    ///     create basket that finalizing the cachedBasket and reserving product for a while
    /// </summary>
    /// <param name="cachedBasketId">cachedBasket id that going to use for getting products of cachedBasket</param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> Create([FromQuery] Guid cachedBasketId)
    {
        var cachedBasket = await _cachingService.GetCachedBasketByIdAsync(cachedBasketId);

        // checking if basket is available
        if (cachedBasket == null) return NotFound($"CachedBasket with id: {cachedBasketId} not found.");


        var cachedProducts = _cachingService.GetAllCachedProducts(cachedBasket);

        // check if cacheBasket has any values
        if (cachedProducts == null || !cachedProducts.Any())
            return BadRequest($"cachedBasket with id: {cachedBasketId} is empty");

        // getting paginatedResult of products that cachedBasket contains
        var paginateProducts = await _productRepository.GetListAsync(new ProductFilter
        {
            Offset = 0,
            Count = int.MaxValue,
            Ids = cachedProducts.Select(x => x.ProductId).ToArray()
        }, HttpContext.RequestAborted);

        var products = paginateProducts.Items;

        // creating main basket object that is going to initialize with cacheBasket products
        var newBasket = new Basket();

        // NOTE: after calling this method and creating a new basket in database, newBasket.Id will initialized
        await _basketRepository.CreateAsync(newBasket, HttpContext.RequestAborted);

        // creating basketProducts object that is going to use for createRange of basketProduct database
        var basketProducts = new List<BasketProduct>();

        foreach (var product in products)
        {
            var cachedProductCountWithSameId = cachedProducts.FirstOrDefault(x => x.ProductId == product.Id).Count;

            // if product.stock is less than requested count of it in the cachedProduct, well for now, just do nothing and not to reduce its stock
            if (product.Stock < cachedProductCountWithSameId)
                //todo: it should return some error messages that x product doesnt have enough stock for you count of product
                // continue if stock of product is less than cachedProduct count.
                continue;
            // this changing will updating the rows in the database using Update method later
            product.Stock -= cachedProductCountWithSameId;

            basketProducts.Add(new BasketProduct
            {
                BasketId = newBasket.Id,
                ProductId = product.Id,
                Count = cachedProductCountWithSameId
            });
        }

        // checking if none of the cachedProducts has non-valid counts of products, return a badrequest to the client
        if (basketProducts.Count == 0)
        {
            await _basketRepository.DeleteAsync(newBasket, HttpContext.RequestAborted);
            return BadRequest($"None of the products in cacheBasket with id:{cachedBasketId} has invalid counts");
        }

        await _productRepository.UpdateRangeAsync(products, HttpContext.RequestAborted);

        await _basketProductRepository.CreateRangeAsync(basketProducts, HttpContext.RequestAborted);

        return Ok(newBasket.Id);
    }


    /// <summary>
    ///     get basket using id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BasketResponse>> Get([FromRoute] Guid id)
    {
        var basket = await _basketRepository.GetAsync(id, HttpContext.RequestAborted);

        if (basket == null) return NotFound($"basket with id: {id} not found.");

        return Ok(_mapper.Map<Basket, BasketResponse>(basket));
    }


    /// <summary>
    ///     delete basket using id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteBasket([FromRoute] Guid id)
    {
        var basket = await _basketRepository.GetAsync(id, HttpContext.RequestAborted);

        if (basket == null) return NotFound();

        var products = basket.Products.Select(x => x.Product);

        foreach (var basketProduct in basket.Products)
        {
            basketProduct.Product.Stock += basketProduct.Count;
        }

        // updating stock in the reserved products
        await _productRepository
            .UpdateRangeAsync(basket.Products.Select(x => x.Product).ToList(), HttpContext.RequestAborted);

        // deleting basket products of the basket in the database
        await _basketProductRepository.DeleteRangeAsync(basket.Products.ToList(), HttpContext.RequestAborted);

        // deleting basket form the database
        await _basketRepository.DeleteAsync(basket, HttpContext.RequestAborted);

        return Ok();
    }


    /// <summary>
    ///     get a basketProduct using id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="productId"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}/products/{productId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BasketProductResponse>> GetBasketProductInfo([FromRoute] Guid id,
        [FromRoute] Guid productId)
    {
        var basket = await _basketRepository.GetAsync(id, HttpContext.RequestAborted);

        if (basket == null) return NotFound($"Basket with id: {id} not found");

        var basketProduct = await _basketProductRepository
            .GetProductByBasketIdAsync(id, productId, HttpContext.RequestAborted);

        if (basketProduct == null)
            return NotFound($"basketProductId with id: {basketProduct} not found");

        return Ok(_mapper.Map<BasketProduct, BasketProductResponse>(basketProduct));
    }


    /// <summary>
    ///     get basketProducts
    /// </summary>
    /// <param name="id"></param>
    /// <param name="offset"></param>
    /// <param name="count"></param>
    /// <param name="productIds"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}/products")]
    public async Task<ActionResult<PaginatedResponseResult<BasketProductResponse>>> GetBasketProducts(
        [FromRoute] Guid id,
        [FromQuery] int offset = 0,
        [FromQuery] int count = 10,
        [FromQuery] Guid[]? productIds = null)
    {
        var basket = await _basketRepository.GetAsync(id, HttpContext.RequestAborted);

        if (basket == null) return NotFound($"basket with id: {id} not found.");

        var filter = new BasketProductFilter
        {
            Offset = offset,
            Count = count,
            ProductIds = productIds,
            BasketId = id
        };

        var paginatedBasketProduct = await _basketProductRepository.GetListAsync(filter, HttpContext.RequestAborted);

        if (!paginatedBasketProduct.HasAnyItems()) return NotFound($"No products found for basket with id: {id}");

        var paginatedResponseResult = _mapper
            .Map<PaginatedResponseResult<BasketProductResponse>>(paginatedBasketProduct);

        return Ok(paginatedResponseResult);
    }
}