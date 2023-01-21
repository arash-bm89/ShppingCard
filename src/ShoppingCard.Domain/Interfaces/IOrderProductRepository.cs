using Common.Interfaces;
using ShoppingCard.Domain.Filters;
using ShoppingCard.Domain.Models;

namespace ShoppingCard.Domain.Interfaces;

public interface IOrderProductRepository : IBaseRepository<OrderProduct, OrderProductFilter>
{
    Task<OrderProduct?> GetProductByBasketIdAsync(Guid orderId, Guid productId, CancellationToken cancellationToken);
}