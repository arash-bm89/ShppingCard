using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCard.Domain.Models;

namespace ShoppingCard.Service.IServices
{
    public interface ICachingService
    {
        Task<CachedBasket?> GetCachedBasketByIdAsync(Guid basketId);
        CachedProduct? GetCachedProductByCachedBasket(CachedBasket basket, Guid productId);
        Task StoreAsync(Guid id, CachedBasket basket);
        void AddCachedProductToBasket(CachedBasket basket, CachedProduct product);
        void AddCachedProductToBasket(CachedBasket basket, Guid productId, uint count);
        void DeleteCachedProductInBasket(CachedBasket basket, CachedProduct product);

        void ApplyCountInCachedProduct(CachedProduct product, uint count);
        void ApplyIncrementByOneToCachedProduct(CachedProduct product);

        Task<Guid> CreateCachedBasketAsync();

    }
}
