namespace Ordering.API.EndPoints;

//public record GetOrdersRequest(PaginationRequest PaginationRequest);

/// <summary>
/// Represents the HTTP response containing a paginated list of orders.
/// </summary>
/// <param name="Orders">The paginated result containing order data and metadata.</param>
public record GetOrdersResponse(PaginatedResult<OrderDto> Orders);

/// <summary>
/// Minimal API endpoint for retrieving all orders with pagination support.
/// </summary>
public class GetOrdersEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/orders", async ([AsParameters] PaginationRequest paginationRequest, ISender sender) =>
        {
            var query = new GetOrdersQuery(paginationRequest);
            var result = await sender.Send(query);
            var response = new GetOrdersResponse(result.Orders);

            return Results.Ok(response);
        })
        .WithName("GetOrders")
        .Produces<GetOrdersResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Gets paginated orders")
        .WithDescription("Retrieves a paginated list of all orders in the system.");
    }
}