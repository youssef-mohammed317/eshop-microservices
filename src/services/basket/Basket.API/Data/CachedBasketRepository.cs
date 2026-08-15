using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.API.Data;

public class CachedBasketRepository(IBasketRepository _basketRepository,
    IDistributedCache _cache) : IBasketRepository
{

    public async Task<ShoppingCart> GetBasket(string username, CancellationToken cancellationToken = default)
    {
        var cachedBasket = await _cache.GetStringAsync(username, cancellationToken);
        if (!string.IsNullOrEmpty(cachedBasket))
            return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)!;

        var basket = await _basketRepository.GetBasket(username, cancellationToken);
        await _cache.SetStringAsync(username, JsonSerializer.Serialize(basket));
        return basket;
    }

    public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
    {
        await _basketRepository.StoreBasket(basket, cancellationToken);

        await _cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket));

        return basket;

    }
    public async Task<bool> DeleteBasket(string username, CancellationToken cancellationToken = default)
    {
        await _basketRepository.DeleteBasket(username, cancellationToken);

        await _cache.RemoveAsync(username);

        return true;
    }


}
