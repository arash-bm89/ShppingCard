using ShoppingCard.Domain.Models;

namespace ShoppingCard.Api.Models;

public class OrderResponse
{
    public ICollection<Payment>? Payments { get; set; }

    public ICollection<OrderProductResponse> Products { get; set; }

    public decimal? FinalPrice { get; set; }

    public Payment? ConfirmedPayment => Payments?.SingleOrDefault(x => x.IsConfirmed);
}