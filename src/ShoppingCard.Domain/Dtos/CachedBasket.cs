namespace ShoppingCard.Domain.Dtos;

public class CachedBasket
{
    public Guid Id { get; set; }

    public ICollection<CachedProduct> CachedProducts { get; set; } = new List<CachedProduct>();
}