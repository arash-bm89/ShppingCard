using Common.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingCard.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.ModelConfigurations;

namespace ShoppingCard.Repository.EntityConfigurations
{
    public class BasketConfiguration : ModelBaseConfiguration<Basket>
    {
        public override void DerivedConfigure(EntityTypeBuilder<Basket> builder)
        {
            builder.HasMany(x => x.Payments)
                .WithOne(x => x.Basket)
                .HasForeignKey(x => x.BasketId);
        }
    }
}
