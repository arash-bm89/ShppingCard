using Common.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingCard.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.ModelConfigurations;
using Microsoft.EntityFrameworkCore;

namespace ShoppingCard.Repository.EntityConfigurations
{
    public class BasketConfiguration : ModelBaseConfiguration<Basket>
    {
        public override void DerivedConfigure(EntityTypeBuilder<Basket> builder)
        {
            builder.Property(x => x.Version);
            builder.HasMany(x => x.Payments)
                .WithOne(x => x.Basket)
                .HasForeignKey(x => x.BasketId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
