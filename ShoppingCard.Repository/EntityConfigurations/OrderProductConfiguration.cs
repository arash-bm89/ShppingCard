using Common.ModelConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingCard.Domain.Models;

namespace ShoppingCard.Repository.EntityConfigurations;

public class OrderProductConfiguration : ModelBaseConfiguration<OrderProduct>
{
    public override void DerivedConfigure(EntityTypeBuilder<OrderProduct> builder)
    {
        builder.HasOne(x => x.Order)
            .WithMany(x => x.Products)
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Product)
            .WithMany(x => x.OrderProducts)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}