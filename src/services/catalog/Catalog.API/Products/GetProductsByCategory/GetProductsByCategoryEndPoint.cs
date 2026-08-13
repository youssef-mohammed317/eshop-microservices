namespace Catalog.API.Products.GetProductsByCategory;

//public record GetProductsByCategoryRequest(List<string> Category);
public record GetProductsByCategoryResponse(IEnumerable<Product> Products);

public class GetProductsByCategoryEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/category/{category}",
            async (string category, ISender sender) =>
            {
                var query = new GetProductsByCategoryQuery(category);

                var result = await sender.Send(query);

                var response = result.Adapt<GetProductsByCategoryResponse>();

                return Results.Ok(response);
            })
            .WithName("GetProductsByCategory")
            .Produces<GetProductsByCategoryResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Products By Category")
            .WithDescription("Get Products By Category")
            .WithTags("Products"); // Groups endpoints together in the Swagger UI
    }
}
