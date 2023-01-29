using System.Security.Cryptography.X509Certificates;
using Common.ExtensionMethods;
using Common.Implementations;
using Microsoft.EntityFrameworkCore;
using ShoppingCard.Domain.Filters;
using ShoppingCard.Domain.Interfaces;
using ShoppingCard.Domain.Models;

namespace ShoppingCard.Repository.Implementations;

public class OrderRepository : BaseRepository<Order, OrderFilter>, IOrderRepository
{
    public OrderRepository(ApplicationDbContext db) : base(db)
    {
    }

    protected override IQueryable<Order> ConfigureInclude(IQueryable<Order> query)
    {
        return query.Include(x => x.Products).ThenInclude(x => x.Product);
    }

    protected override IQueryable<Order> ConfigureListInclude(IQueryable<Order> query)
    {
        return query.Include(x => x.Products).ThenInclude(x => x.Product);
    }

    protected override IQueryable<Order> ApplyFilter(IQueryable<Order> query, OrderFilter filter)
    {
        if (filter.UserId != default)
        {
            query = query.Where(x => filter.UserId == x.UserId);
        }

        return query;
    }

    public async Task<Order?> GetAsync(Guid userId, Guid id, CancellationToken cancellationToken)
    {
        return await _dbSet
            .Apply(ConfigureInclude)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UserId == userId && x.Id == id, cancellationToken);
    }
}