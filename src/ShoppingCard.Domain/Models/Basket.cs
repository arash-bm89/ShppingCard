using System.ComponentModel.DataAnnotations.Schema;
using Common.Models;

namespace ShoppingCard.Domain.Models;

public class Basket : ModelBase
{
    /// <summary>
    ///     using for efforts for paying the basket price
    /// </summary>
    public ICollection<Payment> Payments { get; set; }

    /// <summary>
    ///     using for the confirmed payment for the basket
    /// </summary>
    [NotMapped]
    public Payment? ConfirmedPayment => Payments?.Single(x => x.IsConfirmed);

    /// <summary>
    ///     using for many to many relationship between basket and product
    /// </summary>
    public ICollection<BasketProduct> Products { get; set; }

    /// <summary>
    ///     getting whole price after the task been calculated
    /// </summary>
    [NotMapped]
    public decimal? FinalPrice => Products?.ToList().Sum(x => x.TotalPrice);
}