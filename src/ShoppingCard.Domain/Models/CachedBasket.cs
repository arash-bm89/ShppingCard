using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace ShoppingCard.Domain.Models
{
    public class CachedBasket
    {
        public Guid Id { get; set; }
        public ICollection<CachedProduct> CachedProducts { get; set; } = new List<CachedProduct>();

    }

}
