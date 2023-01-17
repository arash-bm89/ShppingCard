using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCard.Domain.Models;
using ShoppingCard.Service.IServices;

namespace ShoppingCard.Service.Services
{
    public class BasketProductService: IBasketProductService
    {
        public async Task AddBasketProductAsync(BasketProduct basketProduct, CancellationToken cancellationToken)
        {
            
        }
    }
}
