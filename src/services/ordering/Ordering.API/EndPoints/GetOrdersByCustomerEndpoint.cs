namespace Ordering.API.EndPoints;

//public record GetOrdersByCustomerRequest(Guid CustomerId);

/// <summary>
/// Represents the HTTP response containing a list of orders for a specific customer.
/// </summary>
/// <param name="Orders">The list of orders belonging to the customer.</param>
public record GetOrdersByCustomerResponse(IEnumerable<OrderDto> Orders);

/// <summary>
/// Minimal API endpoint for retrieving orders by customer ID.
/// </summary>
public class GetOrdersByCustomerEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/orders/customer/{customerId:guid}", async (Guid customerId, ISender sender) =>
        {
            var query = new GetOrdersByCustomerQuery(customerId);
            var result = await sender.Send(query);
            var response = new GetOrdersByCustomerResponse(result.Orders);

            return Results.Ok(response);
        })
        .WithName("GetOrdersByCustomer")
        .Produces<GetOrdersByCustomerResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Gets orders by customer ID")
        .WithDescription("Retrieves all orders associated with a specific customer.");
    }
}
