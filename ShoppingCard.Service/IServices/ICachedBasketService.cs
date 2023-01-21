using ShoppingCard.Api.Models;
using ShoppingCard.Domain.Dtos;
using ShoppingCard.Domain.Models;

namespace ShoppingCard.Service.IServices;

public interface ICachedBasketService
{
    Task<CachedBasket?> GetCachedBasketByIdAsync(Guid basketId);
    CachedProduct? GetCachedProductByCachedBasket(CachedBasket basket, Guid productId);
    List<CachedProduct>? GetAllCachedProducts(CachedBasket basket);
    Task<CachedBasketDto> GetProductsFromRepositoryAsync(CachedBasket basket, CancellationToken cancellationToken);
    Task<CachedProductDto> GetProductFromRepositoryAsync(CachedProduct product, CancellationToken cancellationToken);
    Task StoreAsync(Guid id, CachedBasket basket);
    void AddCachedProductToBasket(CachedBasket basket, CachedProduct product);
    void AddCachedProductToBasket(CachedBasket basket, Guid productId, uint count);
    void DeleteCachedProductInBasket(CachedBasket basket, CachedProduct product);

    Task DeleteCachedBasket(Guid basketId);

    void ApplyCountInCachedProduct(CachedProduct product, uint count);
    void ApplyIncrementByOneToCachedProduct(CachedProduct product);

    Task<Guid> CreateCachedBasketAsync();
}