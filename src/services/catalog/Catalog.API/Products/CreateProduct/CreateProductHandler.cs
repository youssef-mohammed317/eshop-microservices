namespace Catalog.API.Products.CreateProduct;

public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price)
    : ICommand<CreateProductResult>;
public record CreateProductResult(Guid Id);

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
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


public class CreateProductCommandHandler(IDocumentSession _session)
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        // product
        var product = command.Adapt<Product>();

        // save to database
        _session.Store(product);
        await _session.SaveChangesAsync(cancellationToken);

        // return result
        return new CreateProductResult(product.Id);
    }
}
