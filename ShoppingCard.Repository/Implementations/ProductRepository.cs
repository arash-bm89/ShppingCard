using Common.ExtensionMethods;
using Common.Implementations;
using ShoppingCard.Domain.Filters;
using ShoppingCard.Domain.Interfaces;
using ShoppingCard.Domain.Models;

namespace ShoppingCard.Repository.Implementations;

public class ProductRepository : BaseRepository<Product, ProductFilter>, IProductRepository
{
    public ProductRepository(ApplicationDbContext db) : base(db)
    {
    }

    protected override IQueryable<Product> ConfigureInclude(IQueryable<Product> query)
    {
        return query;
    }

    protected override IQueryable<Product> ConfigureListInclude(IQueryable<Product> query)
    {
        return query;
    }

    protected override IQueryable<Product> ApplyFilter(IQueryable<Product> query, ProductFilter filter)
    {
        if (!string.IsNullOrEmpty(filter.Name)) query = query.Where(x => x.Name.Contains(filter.Name));

        if (filter.IsAvailable != null)
            query = filter.IsAvailable == true
                ? query.Where(x => x.Stock != 0)
                : query.Where(x => x.Stock == 0);

        if (filter.Ids != null && filter.Ids.Any()) query = query.Where(x => filter.Ids.Contains(x.Id));

        query.Apply(OrderByIsAvailable);

        return query;
    }

    public IQueryable<Product> OrderByIsAvailable(IQueryable<Product> query)
    {
        return query.OrderByDescending(x => x.IsAvailable);
    }
}