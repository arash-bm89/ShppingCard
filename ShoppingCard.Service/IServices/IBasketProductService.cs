using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCard.Domain.Models;

namespace ShoppingCard.Service.IServices
{
    public interface IBasketProductService
    {
        Task AddBasketProductAsync(BasketProduct basketProduct, CancellationToken cancellationToken);
    }
}
