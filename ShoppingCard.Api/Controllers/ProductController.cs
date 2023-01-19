using AutoMapper;
using Common.ExtensionMethods;
using Common.Models;
using Microsoft.AspNetCore.Mvc;
using ShoppingCard.Api.Models;
using ShoppingCard.Domain.Filters;
using ShoppingCard.Domain.Interfaces;
using ShoppingCard.Domain.Models;

namespace ShoppingCard.Api.Controllers
{
    // CHECKED

    /// <summary>
    /// actions of products in the database
    /// </summary>
    [ApiController]
    [Route("products")]
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
        /// get product using id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductResponse>> Get(Guid id)
        {
            var product = await _productRepository.GetAsync(id, HttpContext.RequestAborted);

            if (product == null)
            {
                return NotFound($"product with id: {id} not found");
            }

            return Ok(_mapper.Map<ProductResponse>(product));
        }


        /// <summary>
        /// create product
        /// </summary>
        /// <param name="productRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductResponse>> Create(ProductRequest productRequest)
        {
            if (productRequest.Price == 0 || productRequest.Stock == 0)
            {
                return BadRequest("Price or stock can not be 0.");
            }

            bool isAny = await _productRepository
                .HasAnyAsync(x => x.Name == productRequest.Name, HttpContext.RequestAborted);

            if (isAny)
            {
                return BadRequest($"Product with the name: {productRequest.Name} has been used.");
            }
            
            var product = _mapper.Map<Product>(productRequest);
            await _productRepository.CreateAsync(product, HttpContext.RequestAborted);
            
            return Ok(_mapper.Map<ProductResponse>(product));
        }


        /// <summary>
        /// update a product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productRequest"></param>
        /// <returns></returns>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ProductResponse>> Update(Guid id, ProductRequest productRequest)
        {
            if (productRequest.Price == 0 || productRequest.Stock == 0)
            {
                return BadRequest("Price or stock can not be 0.");
            }
            var product = await _productRepository.GetAsync(id, HttpContext.RequestAborted);

            if (product == null)
            {
                return NotFound($"product with id: {id} not found");
            }

            if ((productRequest.Name != product.Name) &&
                await _productRepository.HasAnyAsync(x => x.Name == productRequest.Name, HttpContext.RequestAborted))
            {
                return BadRequest($"Product with the name: {productRequest.Name} is already available");
            }

            var updatedProduct = _mapper.Map<Product>(productRequest);
            product.SetBaseModelPropsToRequest(updatedProduct);
            await _productRepository.UpdateAsync(updatedProduct, HttpContext.RequestAborted);
            return Ok();
        }


        /// <summary>
        /// delete product by id
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



        /// <summary>
        /// get products
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="name"></param>
        /// <param name="isAvailable"></param>
        /// <returns></returns>
        [HttpGet("list")]
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
            // todo: somehow return paginatedResult<productRequest>
            return Ok(paginatedProducts);
        }
    }
}
