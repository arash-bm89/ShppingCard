using Common.Interfaces;
using ShoppingCard.Domain.Filters;
using ShoppingCard.Domain.Models;

namespace ShoppingCard.Domain.Interfaces;

public interface IOrderRepository : IBaseRepository<Order, OrderFilter>
{
    Task<Order?> GetAsync(Guid userId, Guid id, CancellationToken cancellationToken);
}