namespace Shopping.Web.Services;

public interface IBasketService
{
    [Get("/basket-api/basket/{userName}")]
    Task<GetBasketResponse> GetBasket(string userName);

    [Post("/basket-api/basket")]
    Task<StoreBasketResponse> StoreBasket(StoreBasketRequest request);

    [Delete("/basket-api/basket/{userName}")]
    Task<DeleteBasketResponse> DeleteBasket(string userName);

    [Post("/basket-api/basket/checkout")]
    Task<CheckoutBasketResponse> CheckoutBasket(CheckoutBasketRequest request);
}
