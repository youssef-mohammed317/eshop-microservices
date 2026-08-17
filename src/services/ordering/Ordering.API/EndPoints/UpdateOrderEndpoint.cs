namespace Ordering.API.EndPoints;


/// <summary>
/// Represents the HTTP request body for updating an existing order.
/// </summary>
/// <param name="Order">The updated order data transfer object.</param>
public record UpdateOrderRequest(OrderDto Order);

/// <summary>
/// Represents the HTTP response returned after updating an order.
/// </summary>
/// <param name="IsSuccess">Indicates whether the update operation was successful.</param>
public record UpdateOrderResponse(bool IsSuccess);

/// <summary>
/// Minimal API endpoint for updating an existing order.
/// </summary>
public class UpdateOrderEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/orders", async (UpdateOrderRequest request, ISender sender) =>
        {
            var command = new UpdateOrderCommand(request.Order);
            var result = await sender.Send(command);
            var response = new UpdateOrderResponse(result.IsSuccess);

            return Results.Ok(response);
        })
        .WithName("UpdateOrder")
        .Produces<UpdateOrderResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Updates an existing order")
        .WithDescription("Updates an existing order using the provided data.");
    }
}
