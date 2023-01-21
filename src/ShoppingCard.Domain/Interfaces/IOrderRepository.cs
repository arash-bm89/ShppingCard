using Common.Interfaces;
using ShoppingCard.Domain.Filters;
using ShoppingCard.Domain.Models;

namespace ShoppingCard.Domain.Interfaces;

public interface IOrderRepository : IBaseRepository<Order, OrderFilter>
{
}