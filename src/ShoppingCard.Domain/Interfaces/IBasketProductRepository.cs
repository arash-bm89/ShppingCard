using Common.Interfaces;
using ShoppingCard.Domain.Filters;
using ShoppingCard.Domain.Models;

namespace ShoppingCard.Domain.Interfaces;

public interface IBasketProductRepository : IBaseRepository<BasketProduct, BasketProductFilter>
{
    Task<BasketProduct?> GetProductByBasketIdAsync(Guid basketId, Guid productId, CancellationToken cancellationToken);
}