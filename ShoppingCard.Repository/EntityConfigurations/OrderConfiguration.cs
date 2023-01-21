using Common.ModelConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingCard.Domain.Models;

namespace ShoppingCard.Repository.EntityConfigurations;

public class OrderConfiguration : ModelBaseConfiguration<Order>
{
    public override void DerivedConfigure(EntityTypeBuilder<Order> builder)
    {
        builder.Property(x => x.Version);
        builder.HasMany(x => x.Payments)
            .WithOne(x => x.Order)
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}