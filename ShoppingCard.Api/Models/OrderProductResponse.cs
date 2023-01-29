using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCard.Api.Models;

public class OrderProductResponse
{
    public Guid ProductId { get; set; }

    public ProductResponse Product { get; set; }


    /// <summary>
    ///     count of this productRepository in basketRepository
    /// </summary>
    public uint Count { get; set; }

    /// <summary>
    ///     price of all Products of this type in a basket
    /// </summary>
    public decimal TotalPrice { get; set; }
}