using Common.Implementations;
using Microsoft.EntityFrameworkCore;
using ShoppingCard.Domain.Filters;
using ShoppingCard.Domain.Interfaces;
using ShoppingCard.Domain.Models;

namespace ShoppingCard.Repository.Implementations;

public class BasketProductRepository : BaseRepository<BasketProduct, BasketProductFilter>, IBasketProductRepository
{
    public BasketProductRepository(ApplicationDbContext db) : base(db)
    {
    }

    public async Task<BasketProduct?> GetProductByBasketIdAsync(Guid basketId, Guid productId,
        CancellationToken cancellationToken)
    {
        return await _dbSet
            .Where(x => x.ProductId == productId && x.BasketId == basketId)
            .Include(x => x.Product)
            .FirstOrDefaultAsync(cancellationToken);
    }

    protected override IQueryable<BasketProduct> ConfigureInclude(IQueryable<BasketProduct> query)
    {
        return query.Include(x => x.Product);
    }

    protected override IQueryable<BasketProduct> ConfigureListInclude(IQueryable<BasketProduct> query)
    {
        return query.Include(x => x.Product);
    }

    protected override IQueryable<BasketProduct> ApplyFilter(IQueryable<BasketProduct> query,
        BasketProductFilter filter)
    {
        if (filter.ProductIds != null && filter.ProductIds.Any()) query = query.Where(x => filter.ProductIds.Contains(x.ProductId));

        if (filter.BasketId != null) query = query.Where(x => x.BasketId == filter.BasketId);

        return query;
    }
}