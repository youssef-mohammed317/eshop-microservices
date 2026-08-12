namespace Catalog.API.Products.DeleteProduct;

public record DeleteProductCommand(Guid Id)
    : ICommand<DeleteProductResult>;
public record DeleteProductResult(bool IsSuccess);

public class DeleteProductCommandHandler(IDocumentSession _session,
    ILogger<DeleteProductCommandHandler> _logger) : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("DeleteProductCommandHandler.Handle called with {@command}", command);

        // save to database
        _session.Delete<Product>(command.Id);
        await _session.SaveChangesAsync(cancellationToken);

        // return result
        return new DeleteProductResult(true);
    }
}
