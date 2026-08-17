namespace Ordering.API.EndPoints;


//public record GetOrdersByNameRequest(string OrderName);

/// <summary>
/// Represents the HTTP response containing a list of orders filtered by name.
/// </summary>
/// <param name="Orders">The list of matching orders.</param>
public record GetOrdersByNameResponse(IEnumerable<OrderDto> Orders);


/// <summary>
/// Minimal API endpoint for retrieving orders by their name.
/// </summary>
public class GetOrdersByNameEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/orders/{orderName}", async (string orderName, ISender sender) =>
        {
            var query = new GetOrdersByNameQuery(orderName);
            var result = await sender.Send(query);
            var response = new GetOrdersByNameResponse(result.Orders);

            return Results.Ok(response);
        })
        .WithName("GetOrdersByName")
        .Produces<GetOrdersByNameResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Gets orders by name")
        .WithDescription("Retrieves a list of orders that match the specified name.");
    }
}
