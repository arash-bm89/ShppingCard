using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Implementations;
using Microsoft.EntityFrameworkCore;
using ShoppingCard.Domain.Filters;
using ShoppingCard.Domain.Models;

namespace ShoppingCard.Repository.Implementations
{
    public class UserRepository : BaseRepository<User, UserFilter>
    {
        public UserRepository(ApplicationDbContext db) : base(db)
        {
        }

        protected override IQueryable<User> ConfigureInclude(IQueryable<User> query)
        {
            return query.Include(x => x.Orders).ThenInclude(x => x.Payments);
        }

        protected override IQueryable<User> ConfigureListInclude(IQueryable<User> query)
        {
            return query;
        }

        protected override IQueryable<User> ApplyFilter(IQueryable<User> query, UserFilter filter)
        {
            return query;
        }
    }
}
