namespace Ordering.Domain.ValueObjects; // Adjust namespace if necessary

public record OrderId
{
    public Guid Value { get; }

    // Private constructor to force the use of the factory method
    private OrderId(Guid value) => Value = value;

    /// <summary>
    /// Factory method to create a new OrderId instance.
    /// </summary>
    /// <param name="value">The Guid value for the ID.</param>
    /// <returns>A new OrderId instance.</returns>
    /// <exception cref="ArgumentException">Thrown when the provided Guid is empty.</exception>
    public static OrderId Of(Guid value)
    {
        // Validation: Ensure the Guid is not empty
        if (value == Guid.Empty)
        {
            throw new DomainException("Order ID cannot be empty.");
        }

        return new OrderId(value);
    }
}