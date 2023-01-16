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
    public class BasketProductConfiguration : ModelBaseConfiguration<BasketProduct>
    {
        public override void DerivedConfigure(EntityTypeBuilder<BasketProduct> builder)
        {
            builder.HasOne(x => x.Basket)
                .WithMany(x => x.BasketProducts)
                .HasForeignKey(x => x.BasketId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Product)
                .WithMany(x => x.BasketProducts)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
