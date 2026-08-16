namespace Ordering.Domain.ValueObjects;

public record ProductId
{
    public Guid Value { get; }

    // Private constructor to force the use of the factory method
    private ProductId(Guid value) => Value = value;

    /// <summary>
    /// Factory method to create a new ProductId instance.
    /// </summary>
    /// <param name="value">The Guid value for the ID.</param>
    /// <returns>A new ProductId instance.</returns>
    /// <exception cref="DomainException">Thrown when the provided Guid is empty.</exception>
    public static ProductId Of(Guid value)
    {
        // Validation: Ensure the Guid is not empty
        if (value == Guid.Empty)
        {
            throw new DomainException("Product ID cannot be empty.");
        }

        return new ProductId(value);
    }
}