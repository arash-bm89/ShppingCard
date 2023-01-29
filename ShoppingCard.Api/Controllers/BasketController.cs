using Athena.CacheHelper;
using Microsoft.AspNetCore.Mvc;
using ShoppingCard.Api.Models;
using ShoppingCard.Domain.Dtos;
using ShoppingCard.Domain.Interfaces;
using ShoppingCard.Service.IServices;

namespace ShoppingCard.Api.Controllers;

// todo: add "" from some notFound or badRequest responses

/// <summary>
///     cacheBasket is the temporary template using for crud operation on the Products that user want to buy
/// </summary>
[Route("user/basket")]
[ApiController]
public class BasketController : ControllerBase
{
    private readonly ICachedBasketService _cachedBasketService;
    private readonly IProductRepository _productRepository;
    private readonly IJwtService _jwtService;

    public BasketController(ICacheHelper cacheHelper,
        IProductRepository productRepository,
        ICachedBasketService cachedBasketService,
        IJwtService jwtService)
    {
        _productRepository = productRepository;
        _cachedBasketService = cachedBasketService;
        _jwtService = jwtService;
    }


    /// <summary>
    ///     create a Basket and returns id of it.
    /// </summary>
    /// <returns> Guid of new cachedBasket</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateCachedBasket()
    {
        var userId = _jwtService.GetJwtObjectFromHttpContext(HttpContext).Id;

        var noBasket = !await _cachedBasketService.HasAnyByIdAsync(userId);

        if (!noBasket)
            return BadRequest("This User Already Has One Basket.");

        await _cachedBasketService.CreateCachedBasketAsync(userId);
        return Ok();
    }



    /// <summary>
    ///     get Basket
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CachedBasketDto>> GetCachedBasket()
    {
        var userId = _jwtService.GetJwtObjectFromHttpContext(HttpContext).Id;

        var cachedBasket = await _cachedBasketService.GetCachedBasketByIdAsync(userId);

        // checking if cachedBasket is available or not.
        if (cachedBasket == null)
            return NotFound("Basket Not Found");

        var cachedBasketDto = await _cachedBasketService
            .GetProductsFromRepositoryAsync(cachedBasket, HttpContext.RequestAborted);

        return Ok(cachedBasketDto);
    }



    /// <summary>
    ///     updating count of a Product in Basket
    /// </summary>
    /// <param name="id">id of the cachedBasket</param>
    /// <param name="productId"></param>
    /// <param name="count">null means it will be increased by one and 0 will redirected to DeleteAction</param>
    /// <returns></returns>
    [HttpPost("Products/{productId:guid}/")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProductInBasketCache(
        [FromRoute] Guid productId,
        [FromQuery] uint? count = null)
    {
        var userId = _jwtService.GetJwtObjectFromHttpContext(HttpContext).Id;

        // if count of product going to be 0, then the logic is handled in DeleteProduct action
        if (count == 0) return RedirectToAction("DeleteProduct", new { userId, productId });

        var cachedBasket = await _cachedBasketService.GetCachedBasketByIdAsync(userId);

        // checking if cachedBasket is available or not.
        if (cachedBasket == null) return NotFound("Basket Not Found");

        var dbProduct =
            await _productRepository.GetAsync(productId, HttpContext.RequestAborted);

        // checking if product is available in db and if it has stock or not.
        if (dbProduct == null) return NotFound("Product Not Found In Database");

        var product = _cachedBasketService.GetCachedProductByCachedBasket(cachedBasket, productId);

        // if cachedProduct was set already, just apply count in cachedProduct.
        // if count == null update
        if (product != null)
        {
            if (dbProduct.Stock < (count ?? product.Count + 1))
                return BadRequest(
                    $"Count Of Available Products In Database Is {dbProduct.Stock}, Less than {count ?? product.Count + 1}");

            if (count == null)
                _cachedBasketService.ApplyIncrementByOneToCachedProduct(product);
            else
                _cachedBasketService.ApplyCountInCachedProduct(product, (uint)count);
        }

        // if cachedProduct was not set already, setting a new cachedProduct to the cachedProduct
        // and if count was null it will add with count of one
        else
        {
            if (dbProduct.Stock < (count ?? 1))
                return BadRequest(
                    $"Count Of Available Products In Database Is {dbProduct.Stock}, Less than {count ?? 1}");
            if (count == null)
                _cachedBasketService.AddCachedProductToBasket(
                    cachedBasket,
                    productId,
                    1);
            else
                _cachedBasketService.AddCachedProductToBasket(
                    cachedBasket,
                    productId,
                    (uint)count);
        }

        await _cachedBasketService.StoreAsync(userId, cachedBasket);
        return Ok();
    }



    /// <summary>
    ///     get Product of a Basket
    /// </summary>
    /// <param name="id"></param>
    /// <param name="productId"></param>
    /// <returns></returns>
    [HttpGet("Products/{productId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CachedProduct>> GetCachedProduct(
        [FromRoute] Guid productId)
    {
        var userId = _jwtService.GetJwtObjectFromHttpContext(HttpContext).Id;
        var cachedBasket = await _cachedBasketService
            .GetCachedBasketByIdAsync(userId);

        if (cachedBasket == null) return NotFound("Basket Not Found");

        var cachedProduct = _cachedBasketService.GetCachedProductByCachedBasket(cachedBasket, productId);

        if (cachedProduct == null) return NotFound("Product Not Found In The Basket");

        var cachedProductDto = await _cachedBasketService
            .GetProductFromRepositoryAsync(cachedProduct, HttpContext.RequestAborted);


        return Ok(cachedProductDto);
    }



    /// <summary>
    ///     delete a Product from the Basket
    /// </summary>
    /// <param name="id"></param>
    /// <param name="productId"></param>
    /// <returns></returns>
    [HttpDelete("Products/{productId:guid}", Name = "DeleteProduct")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCacheProduct(
        [FromRoute] Guid productId)
    {
        var userId = _jwtService.GetJwtObjectFromHttpContext(HttpContext).Id;
        var cachedBasket = await _cachedBasketService.GetCachedBasketByIdAsync(userId);

        if (cachedBasket == null)
            return NotFound("Basket Not Found");

        var product = _cachedBasketService.GetCachedProductByCachedBasket(cachedBasket, productId);

        if (product == null)
            return NotFound("Product Not Found In The Basket");

        _cachedBasketService.DeleteCachedProductInBasket(cachedBasket, product);

        if (!cachedBasket.CachedProducts.Any())
        {
            await _cachedBasketService.DeleteCachedBasket(cachedBasket.UserId);
            return Ok();
        }

        await _cachedBasketService.StoreAsync(userId, cachedBasket);
        return Ok();
    }
}