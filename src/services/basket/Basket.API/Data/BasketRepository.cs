namespace Basket.API.Data;

public class BasketRepository(IDocumentSession _session) : IBasketRepository
{

    public async Task<ShoppingCart> GetBasket(string username, CancellationToken cancellationToken = default)
    {
        var basket = await _session.LoadAsync<ShoppingCart>(username, cancellationToken);

        return basket ?? throw new BasketNotFoundException(username);
    }

    public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
    {
        _session.Store(basket);
        await _session.SaveChangesAsync(cancellationToken);
        return basket;
    }
    public async Task<bool> DeleteBasket(string username, CancellationToken cancellationToken = default)
    {
        _session.Delete<ShoppingCart>(username);
        await _session.SaveChangesAsync(cancellationToken);
        return true;
    }


}
