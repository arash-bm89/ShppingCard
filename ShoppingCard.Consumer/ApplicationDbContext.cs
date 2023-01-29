using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShoppingCard.BrokerMessage;

namespace ShoppingCard.Consumer
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<LogMessage> LogMessages { get; set; }
        public ApplicationDbContext(DbContextOptions options)
        : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<LogMessage>()
                .HasIndex(x => x.Id).IsUnique();
            builder.Entity<LogMessage>()
                .Property(x => x.Id).UseIdentityColumn().ValueGeneratedOnAdd();
        }
    }
}
