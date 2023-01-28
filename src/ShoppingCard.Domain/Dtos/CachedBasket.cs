namespace ShoppingCard.Domain.Dtos;

/// <summary>
/// object of basket are going to save in redis cache with key of: 'basket_cache - {userId}'
/// </summary>
public class CachedBasket
{
    public Guid UserId { get; set; }
    public ICollection<CachedProduct> CachedProducts { get; set; } = new List<CachedProduct>();
}