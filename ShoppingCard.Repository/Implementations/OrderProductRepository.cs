using Common.Implementations;
using Microsoft.EntityFrameworkCore;
using ShoppingCard.Domain.Filters;
using ShoppingCard.Domain.Interfaces;
using ShoppingCard.Domain.Models;

namespace ShoppingCard.Repository.Implementations;

public class OrderProductRepository : BaseRepository<OrderProduct, OrderProductFilter>, IOrderProductRepository
{
    public OrderProductRepository(ApplicationDbContext db) : base(db)
    {
    }

    public async Task<OrderProduct?> GetProductByBasketIdAsync(Guid orderId, Guid productId,
        CancellationToken cancellationToken)
    {
        return await _dbSet
            .Where(x => x.ProductId == productId && x.OrderId == orderId)
            .Include(x => x.Product)
            .FirstOrDefaultAsync(cancellationToken);
    }

    protected override IQueryable<OrderProduct> ConfigureInclude(IQueryable<OrderProduct> query)
    {
        return query.Include(x => x.Product);
    }

    protected override IQueryable<OrderProduct> ConfigureListInclude(IQueryable<OrderProduct> query)
    {
        return query.Include(x => x.Product);
    }

    protected override IQueryable<OrderProduct> ApplyFilter(IQueryable<OrderProduct> query,
        OrderProductFilter filter)
    {
        if (filter.ProductIds != null && filter.ProductIds.Any())
            query = query.Where(x => filter.ProductIds.Contains(x.ProductId));

        if (filter.OrderId != null) query = query.Where(x => x.OrderId == filter.OrderId);

        return query;
    }
}