
using System.ComponentModel.DataAnnotations;

namespace ShoppingCard.Api.Models
{
    public class ProductRequest
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        [MaxLength(300)]
        public string? Description { get; set; }

        [Required]
        public uint Stock { get; set; }

        public string? ImageUrl { get; set; }
    }
}
