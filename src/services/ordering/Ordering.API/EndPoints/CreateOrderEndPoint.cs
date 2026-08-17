namespace Ordering.API.EndPoints;
/// <summary>
/// Represents the HTTP request body for creating a new order.
/// </summary>
/// <param name="Order">The order data transfer object.</param>
public record CreateOrderRequest(OrderDto Order);

/// <summary>
/// Represents the HTTP response returned after an order is created.
/// </summary>
/// <param name="Id">The unique identifier of the newly created order.</param>
public record CreateOrderResponse(Guid Id);

/// <summary>
/// Minimal API endpoint for creating a new order.
/// </summary>
public class CreateOrderEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/orders", async (CreateOrderRequest request, ISender sender) =>
        {
            var command = new CreateOrderCommand(request.Order);
            var result = await sender.Send(command);
            var response = new CreateOrderResponse(result.Id);

            return Results.Created($"/orders/{response.Id}", response);
        })
        .WithName("CreateOrder")
        .Produces<CreateOrderResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Creates a new order")
        .WithDescription("Creates a new order and returns the generated Order ID.");
    }
}


