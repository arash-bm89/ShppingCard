﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Interfaces;
using ShoppingCard.Domain.Filters;
using ShoppingCard.Domain.Models;

namespace ShoppingCard.Domain.Interfaces
{
    public interface IBasketRepository: IBaseRepository<Basket, BasketFilter>
    {
    }
}
