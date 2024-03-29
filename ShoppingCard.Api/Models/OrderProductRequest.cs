﻿using System.ComponentModel.DataAnnotations;

namespace ShoppingCard.Api.Models;

public class OrderProductRequest
{
    [Required] public Guid ProductId { get; set; }

    /// <summary>
    ///     count of this productRepository in basketRepository
    /// </summary>
    public uint CountOfProduct { get; set; } = 1;
}