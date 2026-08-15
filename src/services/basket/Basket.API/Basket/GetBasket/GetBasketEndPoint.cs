namespace Basket.API.Basket.GetBasket;

public record GetBasketRequest(string UserName);
public record GetBasketResponse(ShoppingCart Cart);

public class GetBasketEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/basket/{username}",
            async (string username, ISender sender) =>
            {
                var query = new GetBasketQuery(username);

                var result = await sender.Send(query);

                var response = result.Adapt<GetBasketResponse>();

                return Results.Ok(response);
            })
            .WithName("GetBasket")
            .Produces<GetBasketResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Basket By Username")
            .WithDescription("Get Basket By Username")
            .WithTags("Basket");
    }
}
