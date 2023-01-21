﻿using Athena.CacheHelper;
using AutoMapper;
using ShoppingCard.Api.Models;
using ShoppingCard.Domain.Dtos;
using ShoppingCard.Domain.Filters;
using ShoppingCard.Domain.Interfaces;
using ShoppingCard.Domain.Models;
using ShoppingCard.Service.IServices;

namespace ShoppingCard.Service.Services;

public class CachedBasketService : ICachedBasketService
{
    private readonly IMapper _mapper;
    private readonly ICacheHelper _cacheHelper;
    private readonly IProductRepository _productRepository;


    public CachedBasketService(ICacheHelper cacheHelper, IProductRepository productRepository, IMapper mapper)
    {
        _cacheHelper = cacheHelper;
        _productRepository = productRepository;
        _mapper = mapper;
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

    public List<CachedProduct>? GetAllCachedProducts(CachedBasket basket)
    {
        var cachedProducts = basket.CachedProducts.ToList();
        return cachedProducts;
    }

    public async Task<CachedBasketDto> GetProductsFromRepositoryAsync(CachedBasket basket, CancellationToken cancellationToken)
    {
        if (!basket.CachedProducts.Any())
            return new CachedBasketDto
            {
                BasketId = basket.Id,
                Products = new List<CachedProductDto>(),
            };

        var products = await _productRepository.GetListAsync(new ProductFilter
        {
            Offset = 0,
            Count = int.MaxValue,
            Ids = basket.CachedProducts.Select(x => x.ProductId).ToArray()
        }, cancellationToken);

        var cachedBasketDto = new CachedBasketDto { };

        cachedBasketDto.BasketId = basket.Id;

        if (products.Items == null || !products.Items.Any())
            return cachedBasketDto;

        foreach (var product in products.Items)
        {
            cachedBasketDto.Products.Add(_mapper.Map<Product, CachedProductDto>(product));

            var cachedProductCount = basket.CachedProducts.First(x => x.ProductId == product.Id).Count;
            cachedBasketDto.Products.Last().Count = cachedProductCount;
        }

        return cachedBasketDto;
    }

    public async Task<CachedProductDto> GetProductFromRepositoryAsync(CachedProduct product, CancellationToken cancellationToken)
    {
        var dbProduct = await _productRepository.GetAsync(product.ProductId, cancellationToken);

        var cachedProductDto = _mapper.Map<Product, CachedProductDto>(dbProduct);
        cachedProductDto.Count = product.Count;

        return cachedProductDto;
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
        basket.CachedProducts.Add(new CachedProduct { ProductId = productId, Count = count });
    }

    public void DeleteCachedProductInBasket(CachedBasket basket, CachedProduct product)
    {
        basket.CachedProducts.Remove(product);
    }

    public async Task DeleteCachedBasket(Guid basketId)
    {
        var key = basketId.ToString();
        await _cacheHelper.RemoveAsync(key);
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

        await _cacheHelper.StoreAsync(key, new CachedBasket { Id = id });

        return id;
    }
}