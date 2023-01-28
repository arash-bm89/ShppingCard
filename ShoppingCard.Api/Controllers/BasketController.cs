using Athena.CacheHelper;
using Microsoft.AspNetCore.Mvc;
using ShoppingCard.Api.Models;
using ShoppingCard.Domain.Dtos;
using ShoppingCard.Domain.Interfaces;
using ShoppingCard.Service.IServices;

namespace ShoppingCard.Api.Controllers;

/// <summary>
///     cacheBasket is the temporary template using for crud operation on the products that user want to buy
/// </summary>
[Route("Basket")]
[ApiController]
// todo: add user for logic of creating basket in the redis.
public class BasketController : ControllerBase
{
    private readonly ICachedBasketService _cachedBasketService;
    private readonly IProductRepository _productRepository;

    public BasketController(ICacheHelper cacheHelper, IProductRepository productRepository,
        ICachedBasketService cachedBasketService)
    {
        _productRepository = productRepository;
        _cachedBasketService = cachedBasketService;
    }


    /// <summary>
    ///     create a Basket and returns id of it.
    /// </summary>
    /// <returns> Guid of new cachedBasket</returns>
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateCachedBasket()
    {
        var id = await _cachedBasketService.CreateCachedBasketAsync();
        return Ok(id);
    }


    /// <summary>
    ///     get Basket
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CachedBasketDto>> GetCachedBasket([FromRoute] Guid id)
    {
        var cachedBasket = await _cachedBasketService.GetCachedBasketByIdAsync(id);

        // checking if cachedBasket is available or not.
        if (cachedBasket == null)
            return NotFound("Cached Order Not Found");

        var cachedBasketDto = await _cachedBasketService
            .GetProductsFromRepositoryAsync(cachedBasket, HttpContext.RequestAborted);

        return Ok(cachedBasketDto);
    }


    /// <summary>
    ///     get Product of a Basket
    /// </summary>
    /// <param name="id"></param>
    /// <param name="productId"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}/products/{productId:guid}")]
    public async Task<ActionResult<CachedProduct>> GetCachedProduct(
        [FromRoute] Guid id,
        [FromRoute] Guid productId)
    {
        var cachedBasket = await _cachedBasketService
            .GetCachedBasketByIdAsync(id);

        if (cachedBasket == null) return NotFound("Cached Order Not Found");

        var cachedProduct = _cachedBasketService.GetCachedProductByCachedBasket(cachedBasket, productId);

        if (cachedProduct == null) return NotFound("Product Not Found In The Cache Of Order");

        var cachedProductDto = await _cachedBasketService
            .GetProductFromRepositoryAsync(cachedProduct, HttpContext.RequestAborted);


        return Ok(cachedProductDto);
    }


    /// <summary>
    ///     updating count of a Product in Basket
    /// </summary>
    /// <param name="id">id of the cachedBasket</param>
    /// <param name="productId"></param>
    /// <param name="count">null means it will be increased by one and 0 will redirected to DeleteAction</param>
    /// <returns></returns>
    [HttpPost("{id:guid}/products/{productId:guid}/")]
    public async Task<IActionResult> UpdateProductInBasketCache(
        [FromRoute] Guid id,
        [FromRoute] Guid productId,
        [FromQuery] uint? count = null)
    {
        if (count == 0) return RedirectToAction("DeleteProduct", new { id, productId });
        var cachedBasket = await _cachedBasketService.GetCachedBasketByIdAsync(id);

        // checking if cachedBasket is available or not.
        if (cachedBasket == null) return NotFound("Cached Order Not Found");

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

        await _cachedBasketService.StoreAsync(id, cachedBasket);
        return Ok();
    }


    /// <summary>
    ///     delete a Product from the Basket
    /// </summary>
    /// <param name="id"></param>
    /// <param name="productId"></param>
    /// <returns></returns>
    [ActionName("DeleteProduct")]
    [HttpDelete("{id:guid}/products/{productId}", Name = "DeleteProduct")]
    public async Task<IActionResult> DeleteCacheProduct(
        [FromRoute] Guid id,
        [FromRoute] Guid productId)
    {
        var cachedBasket = await _cachedBasketService.GetCachedBasketByIdAsync(id);

        if (cachedBasket == null)
            return NotFound("Cached Order Not Found");

        var product = _cachedBasketService.GetCachedProductByCachedBasket(cachedBasket, productId);

        if (product == null)
            return NotFound("Product Not Found In The Cache Of Order");

        _cachedBasketService.DeleteCachedProductInBasket(cachedBasket, product);

        if (!cachedBasket.CachedProducts.Any())
        {
            await _cachedBasketService.DeleteCachedBasket(cachedBasket.Id);
            return Ok();
        }

        await _cachedBasketService.StoreAsync(id, cachedBasket);
        return Ok();
    }
}