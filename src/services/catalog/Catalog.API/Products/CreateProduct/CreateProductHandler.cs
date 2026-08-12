namespace Catalog.API.Products.CreateProduct;

public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price)
    : ICommand<CreateProductResult>;
public record CreateProductResult(Guid Id);

public class CreateProductCommandHandler(IDocumentSession _session,
    ILogger<CreateProductCommandHandler> _logger) : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("CreateProductCommandHandler.Handle called with {@command}", command);

        // product
        var product = command.Adapt<Product>();

        // save to database
        _session.Store(product);
        await _session.SaveChangesAsync(cancellationToken);

        // return result
        return new CreateProductResult(product.Id);
    }
}
