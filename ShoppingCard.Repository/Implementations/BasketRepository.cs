using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Implementations;
using Microsoft.EntityFrameworkCore;
using ShoppingCard.Domain.Interfaces;
using ShoppingCard.Domain.Models;

namespace ShoppingCard.Repository.Implementations
{
    public class BasketRepository: Repository<Basket>, IBasketRepository
    {
        public BasketRepository(ApplicationDbContext db) : base(db)
        {
        }

        protected override IQueryable<Basket> ConfigureInclude(IQueryable<Basket> query)
        {
            return query;
        }

        protected override IQueryable<Basket> ConfigureListInclude(IQueryable<Basket> query)
        {
            return query.Include(x => x.BasketProducts).Include(x => x.Payments);
        }
    }
}
