using Common.Models;

namespace ShoppingCard.Domain.Models;

public class Payment : ModelBase
{
    public Guid OrderId { get; set; }
    public Order Order { get; set; }

    /// <summary>
    ///     if a product is confirmed and user bought the order
    /// </summary>
    public bool IsConfirmed { get; set; }

    /// <summary>
    ///     the time that this payment confirmed at
    /// </summary>
    public DateTimeOffset ConfirmedAt { get; set; }

    /// <summary>
    ///     the time that this order is delivering to the consumer
    /// </summary>
    public DateTimeOffset DeliveryAt { get; set; }
}