using AutoMapper;
using Common.Models;
using Microsoft.AspNetCore.Mvc;
using ShoppingCard.Api.Models;
using ShoppingCard.Domain.Filters;
using ShoppingCard.Domain.Interfaces;
using ShoppingCard.Domain.Models;

namespace ShoppingCard.Api.Controllers
{
    // todo: implementing update endpoint
    // CHECKED

    /// <summary>
    /// actions of products in the database
    /// </summary>
    [ApiController]
    [Route("api/products")]
    public class ProductController: BaseController
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductController(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }


        /// <summary>
        /// get productRepository using id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> Get(Guid id)
        {
            var product = await _productRepository.GetAsync(id, HttpContext.RequestAborted);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }


        /// <summary>
        /// create productRepository using ProductRequest
        /// </summary>
        /// <param name="productRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Create(ProductRequest productRequest)
        {
            var product = _mapper.Map<Product>(productRequest);
            product.Id = Guid.NewGuid();
            await _productRepository.CreateAsync(product, HttpContext.RequestAborted);
            return Ok();
        }


        /// <summary>
        /// delete by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _productRepository.DeleteAsync(id, HttpContext.RequestAborted);
            return Ok();
        }

        [HttpGet("available")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PaginatedResult<Product>>> GetProducts(
            int offset = 0,
            int count = 10,
            string? name = null,
            bool? isAvailable = null
            )
        {
            var paginatedProducts = await _productRepository
                .GetListAsync(new ProductFilter()
                {
                    Offset = offset,
                    Count = count,
                    Name = name,
                    IsAvailable = isAvailable

                }, HttpContext.RequestAborted);

            if (!paginatedProducts.HasAnyItems())
                return NotFound();

            return Ok(paginatedProducts);
        }
    }
}
