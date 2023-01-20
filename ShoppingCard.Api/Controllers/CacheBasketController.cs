using Athena.CacheHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCard.Api.Models;
using ShoppingCard.Domain.Interfaces;
using ShoppingCard.Domain.Models;
using ShoppingCard.Repository.Migrations;
using ShoppingCard.Service.IServices;

namespace ShoppingCard.Api.Controllers
{
    /// <summary>
    /// cacheBasket is the temporary template using for crud operation on the products that user want to buy 
    /// </summary>
    [Route("CacheBasket")]
    [ApiController]
    public class CacheBasketController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ICachingService _cachingService;

        public CacheBasketController(ICacheHelper cacheHelper, IProductRepository productRepository, ICachingService cachingService)
        {
            _productRepository = productRepository;
            _cachingService = cachingService;

        }


        /// <summary>
        /// create a cachedBasket and returns id of it.
        /// </summary>
        /// <returns> Guid of new cachedBasket</returns>
        [HttpPost]
        public async Task<ActionResult<Guid>> CreateCachedBasket()
        {
            var id = await _cachingService.CreateCachedBasketAsync();
            return Ok(id);
        }

        /// <summary>
        /// get cachedBasket
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CachedBasket>> GetCachedBasket(Guid id)
        {
            var cachedBasket = await _cachingService.GetCachedBasketByIdAsync(id);

            // checking if cachedBasket is available or not.
            if (cachedBasket == null)
            {
                return NotFound("Cached Basket Not Found");
            }

            return Ok(cachedBasket);
        }

        /// <summary>
        /// get cachedProduct of a cachedBasket
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet("{id:guid}/products/{productId:guid}")]
        public async Task<ActionResult<CachedProduct>> GetCachedProduct(Guid id, Guid productId)
        {
            var cachedBasket = await _cachingService
                .GetCachedBasketByIdAsync(id);

            if (cachedBasket == null)
            {
                return NotFound("Cached Basket Not Found");
            }

            var product = await _productRepository
                .GetAsync(productId, HttpContext.RequestAborted);

            if (product == null)
            {
                return NotFound("Product Not Found In Database");
            }

            var cachedProduct = _cachingService.GetCachedProductByCachedBasket(cachedBasket, productId);

            if (cachedProduct == null)
            {
                return NotFound("Product Not Found In The Cache Of Basket");
            }

            return Ok(cachedProduct);
        }


        /// <summary>
        /// updating count of a cacheProduct in cacheBasket
        /// </summary>
        /// <param name="id">id of the cachedBasket</param>
        /// <param name="productId"></param>
        /// <param name="count">null means it will be increased by one and 0 will redirected to DeleteAction</param>
        /// <returns></returns>
        [HttpPut("{id:guid}/products/{productId:guid}/")]
        public async Task<IActionResult> UpdateProductInBasketCache(Guid id, Guid productId, uint? count = null)
        {
            if (count == 0)
            {
                return RedirectToAction("DeleteCacheProduct", new {id = id, productId = productId });
            }
            var cachedBasket = await _cachingService.GetCachedBasketByIdAsync(id);

            // checking if cachedBasket is available or not.
            if (cachedBasket == null)
            {
                return NotFound("Cached Basket Not Found");
            }
            
            var dbProduct =
                await _productRepository.GetAsync(productId, HttpContext.RequestAborted);

            // checking if product is available in db and if it has stock or not.
            if (dbProduct == null)
            {
                return NotFound("Product Not Found In Database");
            }

            var product = _cachingService.GetCachedProductByCachedBasket(cachedBasket, productId);

            // if cachedProduct was set already, just apply count in cachedProduct.
            // if count == null update
            if (product != null)
            {
                if (dbProduct.Stock < ((count ?? product.Count + 1)))
                {
                    return BadRequest($"Count Of Available Products In Database Is {dbProduct.Stock}, Less than {count ?? product.Count + 1}");
                }

                if (count == null)
                {
                    _cachingService.ApplyIncrementByOneToCachedProduct(product);
                }
                else
                {
                    _cachingService.ApplyCountInCachedProduct(product, (uint)count);
                }
                
            }

            // if cachedProduct was not set already, setting a new cachedProduct to the cachedProduct
            // and if count was null it will add with count of one
            else 
            {
                if (dbProduct.Stock < ((count ?? 1)))
                {
                    return BadRequest($"Count Of Available Products In Database Is {dbProduct.Stock}, Less than {count ?? 1}");
                }
                if (count == null)
                {
                    _cachingService.AddCachedProductToBasket(
                        cachedBasket,
                        productId,
                        1);
                }
                else
                {
                    _cachingService.AddCachedProductToBasket(
                        cachedBasket,
                        productId,
                        (uint)count);
                }
            }

            await _cachingService.StoreAsync(id, cachedBasket);
            return Ok();

        }

        /// <summary>
        /// delete a cacheProduct from the cacheBasket
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpDelete("{id:guid}/products/{productId}", Name = "DeleteCacheProduct")]
        public async Task<IActionResult> DeleteCacheProduct(Guid id, Guid productId)
        {
            var cachedBasket = await _cachingService.GetCachedBasketByIdAsync(id);

            if (cachedBasket == null)
            {
                return NotFound("Cached Basket Not Found");
            }

            var product = _cachingService.GetCachedProductByCachedBasket(cachedBasket, productId);

            if (product == null)
            {
                return NotFound("Product Not Found In The Cache Of Basket");
            }

            _cachingService.DeleteCachedProductInBasket(cachedBasket, product);
            await _cachingService.StoreAsync(id, cachedBasket);
            return Ok();
        }

    }
}
