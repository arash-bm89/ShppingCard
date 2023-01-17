using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Implementations;
using Microsoft.EntityFrameworkCore;
using ShoppingCard.Domain.Filters;
using ShoppingCard.Domain.Interfaces;
using ShoppingCard.Domain.Models;

namespace ShoppingCard.Repository.Implementations
{
    public class ProductRepository: Repository<Product, ProductFilter>, IProductRepository
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
            if (!string.IsNullOrEmpty(filter.Name))
            {
                query = query.Where(x => x.Name.Contains(filter.Name));
            }
            return query;
        }
    }
}
