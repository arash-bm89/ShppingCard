using Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Common.ModelConfigurations;

public abstract class ModelBaseConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : ModelBase
{
    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        //builder.Property(x => x.Version).HasColumnName("Version");
        builder.HasIndex(x => x.SeqId).IsUnique();
        builder.Property(x => x.SeqId).UseIdentityColumn().ValueGeneratedOnAdd();

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();

        builder.Property(x => x.Version).IsRowVersion();

        builder.Property(x => x.IsDeleted).HasDefaultValue(false);
        builder.HasQueryFilter(x => !x.IsDeleted);

        builder.Property(x => x.CreatedAt).ValueGeneratedOnAdd().HasDefaultValueSql("NOW()");

        DerivedConfigure(builder);
    }

    public abstract void DerivedConfigure(EntityTypeBuilder<TEntity> builder);
}