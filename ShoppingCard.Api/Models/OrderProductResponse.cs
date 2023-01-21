﻿using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCard.Api.Models;

public class OrderProductResponse
{
    public Guid OrderId { get; set; }

    public Guid ProductId { get; set; }

    public ProductResponse Product { get; set; }


    /// <summary>
    ///     count of this productRepository in basketRepository
    /// </summary>
    public uint Count { get; set; }

    /// <summary>
    ///     price of all products of this type in a basket
    /// </summary>
    [NotMapped]
    public decimal TotalPrice => Count * Product.Price;
}