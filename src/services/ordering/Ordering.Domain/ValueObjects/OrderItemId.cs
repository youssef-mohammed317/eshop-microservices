namespace Ordering.Domain.ValueObjects;

public record OrderItemId
{
    public Guid Value { get; }

    // Private constructor to force the use of the factory method
    private OrderItemId(Guid value) => Value = value;

    /// <summary>
    /// Factory method to create a new OrderItemId instance.
    /// </summary>
    /// <param name="value">The Guid value for the ID.</param>
    /// <returns>A new OrderItemId instance.</returns>
    /// <exception cref="DomainException">Thrown when the provided Guid is empty.</exception>
    public static OrderItemId Of(Guid value)
    {
        // Validation: Ensure the Guid is not empty
        if (value == Guid.Empty)
        {
            throw new DomainException("Order Item ID cannot be empty.");
        }

        return new OrderItemId(value);
    }
}