namespace ShoppingCard.Api.Models;

/// <summary>
/// this classes object is going to generate in BasketCachingService
/// </summary>
public class CachedBasketDto
{
    public Guid BasketId { get; set; }

    public ICollection<CachedProductDto> Products { get; set; } = new List<CachedProductDto>();

    public decimal? TotalPrice => Products.Sum(x => x.Count * x.Price);
}