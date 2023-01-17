using System.ComponentModel.DataAnnotations;
using ShoppingCard.Domain.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCard.Api.Models
{
    public class BasketProductRequest
    {
        [Required]
        public Guid ProductId { get; set; }

        /// <summary>
        /// count of this product in basket
        /// </summary>
        public uint CountOfProduct { get; set; } = 1;
    }
}
