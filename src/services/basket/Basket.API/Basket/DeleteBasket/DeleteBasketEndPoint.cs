namespace Basket.API.Basket.DeleteBasket;

//public record DeleteBasketRequest(string UserName);
public record DeleteBasketResponse(bool IsSuccess);
public class DeleteBasketEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/basket/{username}", async (string username, ISender sender) =>
        {
            var command = new DeleteBasketCommand(username);

            var result = await sender.Send(command);

            var response = result.Adapt<DeleteBasketResponse>();

            return Results.Ok(response);
        })
        .WithName("DeleteBasket")
        .Produces<DeleteBasketResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Delete Basket")
        .WithDescription("Delete Basket")
        .WithTags("Basket");
    }
}
