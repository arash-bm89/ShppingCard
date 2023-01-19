using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.ModelConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingCard.Domain.Models;

namespace ShoppingCard.Repository.EntityConfigurations
{
    public class ProductConfiguration: ModelBaseConfiguration<Product>
    {
        public override void DerivedConfigure(EntityTypeBuilder<Product> builder)
        {
            builder.HasIndex(x => x.Name).HasFilter("\"IsDeleted\" = false").IsUnique();
        }
    }
}
