using ShoppingCard.Domain.Models;

namespace ShoppingCard.Api.Models;

public class BasketResponse
{
    public ICollection<Payment>? Payments { get; set; }

    public Payment? ConfirmedPayment => Payments?.SingleOrDefault(x => x.IsConfirmed);

    public ICollection<BasketProductResponse> Products { get; set; }

    public decimal? FinalPrice => Products?.ToList().Sum(x => x.BasketProductPrice);
}