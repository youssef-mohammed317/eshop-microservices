namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductCommand(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price)
    : ICommand<UpdateProductResult>;
public record UpdateProductResult(bool IsSuccess);


public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Product Id is required");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product Name is required");

        RuleFor(x => x.Category)
            .NotEmpty().WithMessage("Product Category is required");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Product Description is required");

        RuleFor(x => x.ImageFile)
            .NotEmpty().WithMessage("Product ImageFile is required");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Product Price must be greater than zero");
    }
}

public class UpdateProductCommandHandler(IDocumentSession _session)
    : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        // Load product
        var product = await _session.LoadAsync<Product>(command.Id, cancellationToken);

        if (product is null)
        {
            throw new ProductNotFoundException(command.Id);
        }

        // Mapster automatically updates the properties of the existing product instance
        command.Adapt(product);

        // Save to database
        _session.Update(product);
        await _session.SaveChangesAsync(cancellationToken);

        // Return result
        return new UpdateProductResult(true);
    }
}
