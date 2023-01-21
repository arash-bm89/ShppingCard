using Common.Implementations;
using Microsoft.EntityFrameworkCore;
using ShoppingCard.Domain.Filters;
using ShoppingCard.Domain.Interfaces;
using ShoppingCard.Domain.Models;

namespace ShoppingCard.Repository.Implementations;

public class BasketRepository : BaseRepository<Basket, BasketFilter>, IBasketRepository
{
    public BasketRepository(ApplicationDbContext db) : base(db)
    {
    }

    protected override IQueryable<Basket> ConfigureInclude(IQueryable<Basket> query)
    {
        return query.Include(x => x.Products).ThenInclude(x => x.Product);
    }

    protected override IQueryable<Basket> ConfigureListInclude(IQueryable<Basket> query)
    {
        return query;
    }

    protected override IQueryable<Basket> ApplyFilter(IQueryable<Basket> query, BasketFilter filter)
    {
        return query;
    }
}