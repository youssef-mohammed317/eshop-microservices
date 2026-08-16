namespace Ordering.Domain.Models;

public class Product : Entity<ProductId>
{
    public string Name { get; private set; } = default!;
    public decimal Price { get; private set; } = default!;

    // Protected parameterless constructor required by Entity Framework Core
    protected Product() { }

    // Private constructor to prevent direct instantiation
    private Product(ProductId id, string name, decimal price)
    {
        Id = id;
        Name = name;
        Price = price;
    }

    /// <summary>
    /// Used to create a new product instance.
    /// </summary>
    public static Product Create(ProductId id, string name, decimal price)
    {
        // Validation rules
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Product name cannot be null or empty.");

        if (price < 0)
            throw new DomainException("Product price cannot be negative.");

        return new Product(id, name, price);
    }

    /// <summary>
    /// Used to reconstitute a product object, usually when retrieving it from the database.
    /// </summary>
    public static Product Of(ProductId id, string name, decimal price)
    {
        return new Product(id, name, price);
    }
}