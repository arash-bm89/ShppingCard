using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Common.Implementations;
using Microsoft.EntityFrameworkCore;
using ShoppingCard.Domain.Filters;
using ShoppingCard.Domain.Interfaces;
using ShoppingCard.Domain.Models;

namespace ShoppingCard.Repository.Implementations
{
    public class BasketProductRepository: BaseRepository<Domain.Models.BasketProduct, BasketProductFilter>, IBasketProductRepository
    {
        public BasketProductRepository(ApplicationDbContext db) : base(db)
        {
        }

        protected override IQueryable<Domain.Models.BasketProduct> ConfigureInclude(IQueryable<Domain.Models.BasketProduct> query)
        {
            return query.Include(x => x.Product).Include(x => x.Product);
        }

        protected override IQueryable<Domain.Models.BasketProduct> ConfigureListInclude(IQueryable<Domain.Models.BasketProduct> query)
        {
            return query.Include(x => x.Product);
        }

        protected override IQueryable<Domain.Models.BasketProduct> ApplyFilter(IQueryable<Domain.Models.BasketProduct> query, BasketProductFilter filter)
        {
            if (filter.Ids != null && filter.Ids.Any())
            {
                query = query.Where(x => filter.Ids.Contains(x.Id));
            }

            if (filter.BasketId != null)
            {
                query = query.Where(x => x.BasketId == filter.BasketId);
            }

            return query;
        }

        public async Task<Domain.Models.BasketProduct?> GetProductByBasketIdAsync(Guid basketId, Guid productId, CancellationToken cancellationToken)
        {
            return await _dbSet
                .Where(x => x.ProductId == productId && x.BasketId == basketId)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
