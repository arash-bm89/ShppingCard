using System.ComponentModel;
using Common.Models;

namespace ShoppingCard.Domain.Filters;

public struct ProductFilter : IListFilter
{
    [DefaultValue(0)] public int Offset { get; set; }

    [DefaultValue(10)] public int Count { get; set; }

    public string? Name { get; set; }

    public bool? IsAvailable { get; set; }
    public Guid[]? Ids { get; set; }
}