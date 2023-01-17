using AutoMapper;
using Common.Models;
using Microsoft.AspNetCore.Mvc;
using ShoppingCard.Api.Models;
using ShoppingCard.Domain.Filters;
using ShoppingCard.Domain.Interfaces;
using ShoppingCard.Domain.Models;

namespace ShoppingCard.Api.Controllers
{
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


        // todo: going to be commented after finalize
        /// <summary>
        /// Get all the products (for testing)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<Product>?> GetAll()
        {
            return await _productRepository.GetAllAsync(HttpContext.RequestAborted);
        }


        /// <summary>
        /// get product using id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> Get(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id, HttpContext.RequestAborted);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }


        /// <summary>
        /// create product using ProductRequest
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

            await _productRepository.DeleteByIdAsync(id, HttpContext.RequestAborted);
            return Ok();
        }

        [HttpGet("TEst")]
        public async Task<ActionResult<PaginatedResult<Product>>> testListQuery([FromQuery] ProductFilter filter)
        {
            return await _productRepository.GetListAsync(filter, HttpContext.RequestAborted);
        }
    }
}
