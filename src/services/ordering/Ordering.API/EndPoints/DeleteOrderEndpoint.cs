namespace Ordering.API.EndPoints;



//public record DeleteOrderRequest(Guid Id);

/// <summary>
/// Represents the HTTP response returned after an order is deleted.
/// </summary>
/// <param name="IsSuccess">Indicates whether the deletion was successful.</param>
public record DeleteOrderResponse(bool IsSuccess);
/// <summary>
/// Minimal API endpoint for deleting an order by its unique identifier.
/// </summary>
public class DeleteOrderEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/orders/{id:guid}", async (Guid id, ISender sender) =>
        {
            var command = new DeleteOrderCommand(id);
            var result = await sender.Send(command);
            var response = new DeleteOrderResponse(result.IsSuccess);

            return Results.Ok(response);
        })
        .WithName("DeleteOrder")
        .Produces<DeleteOrderResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Deletes an order")
        .WithDescription("Deletes an existing order by its ID.");
    }
}
