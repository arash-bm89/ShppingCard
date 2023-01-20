using ShoppingCard.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCard.Api.Models
{
    public class BasketProductResponse
    {
        public Guid BasketId { get; set; }

        public Guid ProductId { get; set; }

        public ProductResponse Product { get; set; }


        /// <summary>
        /// count of this productRepository in basketRepository
        /// </summary>
        public uint CountOfProduct { get; set; }

        /// <summary>
        /// price of all products of this type in a basket
        /// </summary>
        [NotMapped]
        public decimal BasketProductPrice => CountOfProduct * Product.Price;
    }
}
