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
        return query;
    }

    protected override IQueryable<Order> ApplyFilter(IQueryable<Order> query, OrderFilter filter)
    {
        return query;
    }
}