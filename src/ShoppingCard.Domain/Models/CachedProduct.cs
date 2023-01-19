using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ShoppingCard.Domain.Models
{
    public class CachedProduct
    {
        public Guid ProductId { get; set; }
        public uint Count { get; set; }
        
    }
}
