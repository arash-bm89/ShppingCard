using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.ExtensionMethods;
using Common.Implementations;
using Microsoft.EntityFrameworkCore;
using ShoppingCard.Domain.Filters;
using ShoppingCard.Domain.Interfaces;
using ShoppingCard.Domain.Models;

namespace ShoppingCard.Repository.Implementations
{
    public class ProductRepository: BaseRepository<Domain.Models.Product, ProductFilter>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
        }

        protected override IQueryable<Domain.Models.Product> ConfigureInclude(IQueryable<Domain.Models.Product> query)
        {
            return query;
        }

        protected override IQueryable<Domain.Models.Product> ConfigureListInclude(IQueryable<Domain.Models.Product> query)
        {
            return query;
        }

        protected override IQueryable<Domain.Models.Product> ApplyFilter(IQueryable<Domain.Models.Product> query, ProductFilter filter)
        {
            if (!string.IsNullOrEmpty(filter.Name))
            {
                query = query.Where(x => x.Name.Contains(filter.Name));
            }

            if (filter.IsAvailable != null)
            {
                query = filter.IsAvailable == true 
                    ? query.Where(x => x.NumberOfAvailable != 0)
                    : query.Where(x => x.NumberOfAvailable == 0);
            }

            query.Apply(OrderByIsAvailable);

            return query;
        }

        public IQueryable<Product> OrderByIsAvailable(IQueryable<Product> query)
        {
            return query.OrderByDescending(x => x.IsAvailable);
        }
    }
}
