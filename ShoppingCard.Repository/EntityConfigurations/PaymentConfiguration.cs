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
    public class PaymentConfiguration: ModelBaseConfiguration<Payment>
    {
        public override void DerivedConfigure(EntityTypeBuilder<Payment> builder)
        {
            
        }
    }
}
