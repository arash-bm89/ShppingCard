using Common.ModelConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingCard.Domain.Models;

namespace ShoppingCard.Repository.EntityConfigurations;

public class BasketProductConfiguration : ModelBaseConfiguration<BasketProduct>
{
    public override void DerivedConfigure(EntityTypeBuilder<BasketProduct> builder)
    {
        builder.HasOne(x => x.Basket)
            .WithMany(x => x.Products)
            .HasForeignKey(x => x.BasketId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Product)
            .WithMany(x => x.BasketProducts)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}