﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Models;

namespace ShoppingCard.Domain.Filters
{
    // todo: making bpFilter and its ilistInclude and applyFilter
    public struct BasketProductFilter: IListFilter
    {
        public int Offset { get; set; }

        public int Count { get; set; }

        public Guid[]? Ids { get; set; }

        /// <summary>
        /// this is going to use for /baskets/{id}/products/list and we gonna use this field to find basketProducts for this basketId
        /// </summary>
        public Guid? BasketId { get; set; }

    }
}
