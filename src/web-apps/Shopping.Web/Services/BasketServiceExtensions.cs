using System.Net;

namespace Shopping.Web.Services;

public static class BasketServiceExtensions
{
    // أضفنا this IBasketService لكي ترتبط هذه الدالة بالواجهة
    public static async Task<ShoppingCartModel> LoadUserBasket(this IBasketService basketService)
    {
        // Get Basket If Not Exist Create New Basket with Default Logged In User Name: swn
        var userName = "swn";
        ShoppingCartModel basket;

        try
        {
            var getBasketResponse = await basketService.GetBasket(userName);
            basket = getBasketResponse.Cart;
        }
        catch (ApiException apiException) when (apiException.StatusCode == HttpStatusCode.NotFound)
        {
            basket = new ShoppingCartModel
            {
                UserName = userName,
                Items = []
            };
        }

        return basket;
    }
}