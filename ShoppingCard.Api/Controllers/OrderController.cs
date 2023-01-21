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
///     using for actions of final orderRepository
/// </summary>
[ApiController]
[Route("orders")]
public class OrderController : BaseController
{
    private readonly IOrderProductRepository _orderProductRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly ICachedBasketService _cachingService;
    private readonly IMapper _mapper;
    private readonly IProductRepository _productRepository;

    public OrderController(IOrderRepository orderRepository,
        IMapper mapper,
        IOrderProductRepository orderProductRepository,
        ICachedBasketService cachingService,
        IProductRepository productRepository)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
        _orderProductRepository = orderProductRepository;
        _cachingService = cachingService;
        _productRepository = productRepository;
    }


    /// <summary>
    ///     create order that finalizing the Basket and reserving product for a while
    /// </summary>
    /// <param name="basketId">Basket id that going to use for getting products of cachedBasket</param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> Create([FromQuery] Guid basketId)
    {
        var cachedBasket = await _cachingService.GetCachedBasketByIdAsync(basketId);

        // checking if basket is available
        if (cachedBasket == null) return NotFound($"CachedBasket with id: {basketId} not found.");


        var cachedProducts = _cachingService.GetAllCachedProducts(cachedBasket);

        // check if cacheBasket has any values
        if (cachedProducts == null || !cachedProducts.Any())
            return BadRequest($"cachedBasket with id: {basketId} is empty");

        // getting paginatedResult of products that cachedBasket contains
        var paginateProducts = await _productRepository.GetListAsync(new ProductFilter
        {
            Offset = 0,
            Count = int.MaxValue,
            Ids = cachedProducts.Select(x => x.ProductId).ToArray()
        }, HttpContext.RequestAborted);

        var products = paginateProducts.Items;

        // creating main basket object that is going to initialize with cacheBasket products
        var newOrder = new Order()
        {
            Id = Guid.NewGuid(),
        };

        // creating basketProducts object that is going to use for createRange of basketProduct database
        var basketProducts = new List<OrderProduct>();

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

            basketProducts.Add(new OrderProduct
            {
                OrderId = newOrder.Id,
                ProductId = product.Id,
                Count = cachedProductCountWithSameId,
                TotalPrice = product.Price * cachedProductCountWithSameId,
            });
            newOrder.FinalPrice += (product.Price * cachedProductCountWithSameId);
        }

        // checking if none of the cachedProducts has non-valid counts of products, return a badrequest to the client
        if (basketProducts.Count == 0)
        {
            return BadRequest($"None of the products in cacheBasket with id:{basketId} has invalid counts");
        }

        await _productRepository.UpdateRangeAsync(products, HttpContext.RequestAborted);

        // updating finalPrice of newBasket in the database
        await _orderRepository.CreateAsync(newOrder, HttpContext.RequestAborted);

        await _orderProductRepository.CreateRangeAsync(basketProducts, HttpContext.RequestAborted);

        return Ok(newOrder.Id);
    }


    /// <summary>
    ///     get order using id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrderResponse>> Get([FromRoute] Guid id)
    {
        var basket = await _orderRepository.GetAsync(id, HttpContext.RequestAborted);

        if (basket == null) return NotFound($"basket with id: {id} not found.");

        return Ok(_mapper.Map<Order, OrderResponse>(basket));
    }


    /// <summary>
    ///     delete order using id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteBasket([FromRoute] Guid id)
    {
        var basket = await _orderRepository.GetAsync(id, HttpContext.RequestAborted);

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
        await _orderProductRepository.DeleteRangeAsync(basket.Products.ToList(), HttpContext.RequestAborted);

        // deleting basket form the database
        await _orderRepository.DeleteAsync(basket, HttpContext.RequestAborted);

        return Ok();
    }


    /// <summary>
    ///     get a orderProduct using id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="productId"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}/products/{productId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrderProductResponse>> GetBasketProductInfo([FromRoute] Guid id,
        [FromRoute] Guid productId)
    {
        var basket = await _orderRepository.GetAsync(id, HttpContext.RequestAborted);

        if (basket == null) return NotFound($"Order with id: {id} not found");

        var basketProduct = await _orderProductRepository
            .GetProductByBasketIdAsync(id, productId, HttpContext.RequestAborted);

        if (basketProduct == null)
            return NotFound($"basketProductId with id: {basketProduct} not found");

        return Ok(_mapper.Map<OrderProduct, OrderProductResponse>(basketProduct));
    }


    /// <summary>
    ///     get orderProducts
    /// </summary>
    /// <param name="id"></param>
    /// <param name="offset"></param>
    /// <param name="count"></param>
    /// <param name="productIds"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}/products")]
    public async Task<ActionResult<PaginatedResponseResult<OrderProductResponse>>> GetBasketProducts(
        [FromRoute] Guid id,
        [FromQuery] int offset = 0,
        [FromQuery] int count = 10,
        [FromQuery] Guid[]? productIds = null)
    {
        var basket = await _orderRepository.GetAsync(id, HttpContext.RequestAborted);

        if (basket == null) return NotFound($"basket with id: {id} not found.");

        var filter = new OrderProductFilter
        {
            Offset = offset,
            Count = count,
            ProductIds = productIds,
            OrderId = id
        };

        var paginatedBasketProduct = await _orderProductRepository.GetListAsync(filter, HttpContext.RequestAborted);

        if (!paginatedBasketProduct.HasAnyItems()) return NotFound($"No products found for basket with id: {id}");

        var paginatedResponseResult = _mapper
            .Map<PaginatedResponseResult<OrderProductResponse>>(paginatedBasketProduct);

        return Ok(paginatedResponseResult);
    }
}