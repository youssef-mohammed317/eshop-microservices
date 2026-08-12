namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductCommand(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price)
    : ICommand<UpdateProductResult>;
public record UpdateProductResult(bool IsSuccess);

public class UpdateProductCommandHandler(IDocumentSession _session,
    ILogger<UpdateProductCommandHandler> _logger) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {

        _logger.LogInformation("UpdateProductCommandHandler.Handle called with {@command}", command);

        // product
        var product = await _session.LoadAsync<Product>(command.Id, cancellationToken);

        if (product is null)
        {
            throw new ProductNotFoundException();
        }

        product.Name = command.Name;
        product.Category = command.Category;
        product.Description = command.Description;
        product.ImageFile = command.ImageFile;
        product.Price = command.Price;

        // save to database
        _session.Update(product);
        await _session.SaveChangesAsync(cancellationToken);

        // return result
        return new UpdateProductResult(true);
    }
}
