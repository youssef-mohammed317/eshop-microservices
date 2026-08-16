namespace Ordering.Domain.ValueObjects;

public record CustomerId
{
    // Changed 'set' to 'init' to ensure immutability, which is a core rule for Value Objects
    public Guid Value { get; }

    // Private constructor to force the use of the factory method
    private CustomerId(Guid value) => Value = value;

    /// <summary>
    /// Factory method to create a new CustomerId instance.
    /// </summary>
    /// <param name="value">The Guid value for the ID.</param>
    /// <returns>A new CustomerId instance.</returns>
    /// <exception cref="ArgumentException">Thrown when the provided Guid is empty.</exception>
    public static CustomerId Of(Guid value)
    {
        // Validation: Ensure the Guid is not empty
        if (value == Guid.Empty)
        {
            throw new DomainException("Customer ID cannot be empty.");
        }

        return new CustomerId(value);
    }
}