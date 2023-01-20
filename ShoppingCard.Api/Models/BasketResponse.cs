using ShoppingCard.Domain.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCard.Api.Models
{
    public class BasketResponse
    {
        public ICollection<Payment>? Payments { get; set; }

        public Payment? ConfirmedPayment => Payments?.SingleOrDefault(x => x.IsConfirmed);

        public ICollection<BasketProductResponse> BasketProducts { get; set; }

        public decimal? FinalPrice => BasketProducts?.ToList().Sum(x => x.BasketProductPrice);
    }
}
