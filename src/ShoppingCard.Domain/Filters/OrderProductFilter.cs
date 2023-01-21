using Common.Models;

namespace ShoppingCard.Domain.Filters;

public struct OrderProductFilter : IListFilter
{
    public int Offset { get; set; }

    public int Count { get; set; }

    public Guid[]? ProductIds { get; set; }

    /// <summary>
    ///     this is going to use for /orders/{id}/products/list and we gonna use this field to find orderProducts for this
    ///     basketId
    /// </summary>
    public Guid? OrderId { get; set; }
}