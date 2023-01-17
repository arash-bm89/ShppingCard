﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Models;

namespace ShoppingCard.Domain.Filters
{
    public struct BasketProductFilter: IListFilter
    {
        public int Offset { get; set; }
        public int Count { get; set; }
        public Guid[] Ids { get; set; }
    }
}
