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
    public class BasketRepository: BaseRepository<Domain.Models.Basket, BasketFilter>, IBasketRepository
    {
        public BasketRepository(ApplicationDbContext db) : base(db)
        {
        }

        protected override IQueryable<Domain.Models.Basket> ConfigureInclude(IQueryable<Domain.Models.Basket> query)
        {
            return query.Include(x => x.BasketProducts).ThenInclude(x => x.Product);
        }

        protected override IQueryable<Domain.Models.Basket> ConfigureListInclude(IQueryable<Domain.Models.Basket> query)
        {
            return query;
        }

        protected override IQueryable<Domain.Models.Basket> ApplyFilter(IQueryable<Domain.Models.Basket> query, BasketFilter filter)
        {
            return query;
        }
    }
}
