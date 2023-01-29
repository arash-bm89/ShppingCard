using Common.Models;

namespace ShoppingCard.Domain.Filters;

public struct OrderFilter : IListFilter
{
    public int Offset { get; set; }
    public int Count { get; set; }
    public Guid UserId { get; set; }
}