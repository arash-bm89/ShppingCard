using Common.Models;

namespace ShoppingCard.Domain.Filters;

public struct BasketFilter : IListFilter
{
    public int Offset { get; set; }
    public int Count { get; set; }
}