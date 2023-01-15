using Common.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ModelConfigurations
{
    public abstract class ModelBaseConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : ModelBase
    {
        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            
            builder.HasIndex(x => x.SeqId).IsUnique(true);
            builder.Property(x => x.SeqId).UseIdentityColumn().ValueGeneratedOnAdd();

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();

            builder.Property(x => x.RowVersion).IsRowVersion();

            builder.Property(x => x.IsDeleted).HasDefaultValue(false);

            builder.Property(x => x.CreatedAt).ValueGeneratedOnAdd().HasDefaultValueSql("NOW()");

            this.DerivedConfigure(builder);
        }

        public abstract void DerivedConfigure(EntityTypeBuilder<TEntity> builder);
    }
}
