﻿namespace ShoppingCard.Api.Models;

public class ProductResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public decimal Price { get; set; }

    public string? Description { get; set; }

    public uint Stock { get; set; }

    public bool IsAvailable => Stock != 0;


    public string? ImageUrl { get; set; }
}