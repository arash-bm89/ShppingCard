﻿using Common.Models;

namespace ShoppingCard.Domain.Models;

public class Payment : ModelBase
{
    public Guid BasketId { get; set; }
    public Basket Basket { get; set; }

    /// <summary>
    ///     if a product is confirmed and user bought the basket
    /// </summary>
    public bool IsConfirmed { get; set; }

    /// <summary>
    ///     the time that this payment confirmed at
    /// </summary>
    public DateTimeOffset ConfirmedAt { get; set; }

    /// <summary>
    ///     the time that this basket is delivering to the consumer
    /// </summary>
    public DateTimeOffset DeliveryAt { get; set; }
}