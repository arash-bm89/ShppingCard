using Common.Models;

namespace ShoppingCard.Domain.Filters;

public struct BasketProductFilter : IListFilter
{
    public int Offset { get; set; }

    public int Count { get; set; }

    public Guid[]? ProductIds { get; set; }

    /// <summary>
    ///     this is going to use for /baskets/{id}/products/list and we gonna use this field to find basketProducts for this
    ///     basketId
    /// </summary>
    public Guid? BasketId { get; set; }
}