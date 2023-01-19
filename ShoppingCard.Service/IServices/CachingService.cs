using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Athena.CacheHelper;
using ShoppingCard.Domain.Models;
using ShoppingCard.Repository.Implementations;
using StackExchange.Redis;

namespace ShoppingCard.Service.IServices
{
    public class CachingService: ICachingService
    {
        private readonly ICacheHelper _cacheHelper;


        public CachingService(ICacheHelper cacheHelper)
        {
            _cacheHelper = cacheHelper;
            _cacheHelper._options.Value.Prefix = "basket_cache";
        }


        public async Task<CachedBasket?> GetCachedBasketByIdAsync(Guid basketId)
        {
            var key = basketId.ToString();
            return await _cacheHelper.FetchAsync<CachedBasket>(key);
        }

        public CachedProduct? GetCachedProductByCachedBasket(CachedBasket basket, Guid productId)
        {
            return basket.CachedProducts.FirstOrDefault(x => x.ProductId == productId);
        }

        public async Task StoreAsync(Guid id, CachedBasket basket)
        {
            var key = id.ToString();
            await _cacheHelper.StoreAsync(key, basket);
        }

        public void AddCachedProductToBasket(CachedBasket basket, CachedProduct product)
        {
            basket.CachedProducts.Add(product);
        }

        public void AddCachedProductToBasket(CachedBasket basket, Guid productId, uint count)
        {
            basket.CachedProducts.Add(new CachedProduct(){ ProductId = productId, Count = count});
        }

        public void DeleteCachedProductInBasket(CachedBasket basket, CachedProduct product)
        {
            basket.CachedProducts.Remove(product);
        }

        public void ApplyCountInCachedProduct(CachedProduct product, uint count)
        {
            product.Count = count;
        }

        public void ApplyIncrementByOneToCachedProduct(CachedProduct product)
        {
            product.Count++;
        }

        public async Task<Guid> CreateCachedBasketAsync()
        {
            var id = Guid.NewGuid();
            var key = id.ToString();

            await _cacheHelper.StoreAsync(key, new CachedBasket(){Id = id});

            return id;
        }
    }
}
