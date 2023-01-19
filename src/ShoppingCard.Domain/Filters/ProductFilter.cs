using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Common.Models;

namespace ShoppingCard.Domain.Filters
{
    public struct ProductFilter : IListFilter
    {
        [DefaultValue(0)]
        public int Offset { get; set; }

        [DefaultValue(10)]
        public int Count { get; set; }

        public string? Name { get; set; }

        public bool? IsAvailable { get; set; }
    }
}
