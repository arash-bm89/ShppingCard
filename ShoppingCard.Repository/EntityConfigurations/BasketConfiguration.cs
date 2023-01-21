using Common.ModelConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingCard.Domain.Models;

namespace ShoppingCard.Repository.EntityConfigurations;

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