using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Models;

namespace ShoppingCard.Domain.Models;

[Table("Product")]
public class Product : ModelBase
{
    /// <summary>
    ///     name of the product
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }

    /// <summary>
    ///     price of the product
    /// </summary>
    [Required]
    public decimal Price { get; set; }

    [MaxLength(300)] public string? Description { get; set; }

    /// <summary>
    ///     number of available Products
    /// </summary>
    [Required]
    public uint Stock { get; set; }

    [NotMapped] public bool IsAvailable => Stock != 0;


    [MaxLength(100)] public string? ImageUrl { get; set; }

    /// <summary>
    ///     using for many to many relationship between order and product
    /// </summary>

    public ICollection<OrderProduct> OrderProducts { get; set; }
}