using System.ComponentModel.DataAnnotations.Schema;
using Common.Models;

namespace ShoppingCard.Domain.Models;

[Table("OrderProduct")]
public class OrderProduct : ModelBase
{
    public Guid OrderId { get; set; }
    public Order Order { get; set; }
    public Guid ProductId { get; set; }
    public Product Product { get; set; }

    /// <summary>
    ///     count of this product in basket
    /// </summary>
    public uint Count { get; set; }

    /// <summary>
    ///     price of all products of this type in a basket
    /// </summary>
    [NotMapped]
    public decimal TotalPrice => Count * Product.Price;
}