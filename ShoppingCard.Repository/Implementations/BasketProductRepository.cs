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
    public class BasketProductRepository: Repository<BasketProduct, BasketProductFilter>, IBasketProductRepository
    {
        public BasketProductRepository(ApplicationDbContext db) : base(db)
        {
        }

        protected override IQueryable<BasketProduct> ConfigureInclude(IQueryable<BasketProduct> query)
        {
            return query.Include(x => x.Product).Include(x => x.Product);
        }

        protected override IQueryable<BasketProduct> ConfigureListInclude(IQueryable<BasketProduct> query)
        {
            return query;
        }

        protected override IQueryable<BasketProduct> ApplyFilter(IQueryable<BasketProduct> query, BasketProductFilter filter)
        {
            return query;
        }

        public async Task<Product?> GetProductByBasketIdAsync(Guid basketId, Guid productId, CancellationToken cancellationToken)
        {
            return await _dbSet
                .Where(x => x.ProductId == productId && x.BasketId == basketId)
                .Select(x => x.Product).FirstOrDefaultAsync(cancellationToken);
        }
    }
}
