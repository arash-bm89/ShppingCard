using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.ModelConfigurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingCard.Domain.Models;

namespace ShoppingCard.Repository.EntityConfigurations
{
    public class UserConfiguration : ModelBaseConfiguration<User>
    {
        public override void DerivedConfigure(EntityTypeBuilder<User> builder)
        {
            builder.HasIndex(x => x.Name).IsUnique();

            builder.HasIndex(x => x.Email).IsUnique();

            builder.HasMany(x => x.Orders)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);
        }
    }
}
