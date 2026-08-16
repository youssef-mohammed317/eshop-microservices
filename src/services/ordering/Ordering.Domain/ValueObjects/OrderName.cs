namespace Ordering.Domain.ValueObjects;

public record OrderName
{
    private const int _defaultLength = 25;

    public string Value { get; }

    // Private constructor to force the use of the factory method
    private OrderName(string value) => Value = value;

    /// <summary>
    /// Factory method to create a new OrderName instance.
    /// </summary>
    /// <param name="value">The string value for the Order Name.</param>
    /// <returns>A new OrderName instance.</returns>
    /// <exception cref="DomainException">Thrown when the provided string is null or empty.</exception>
    public static OrderName Of(string value)
    {
        // Validation: Ensure the name is not null, empty, or just whitespace
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException("Order Name cannot be null or empty.");
        }


        // You can also add length constraints here if needed, e.g.:
        if (value.Length > _defaultLength)
        {
            throw new DomainException($"Order Name cannot exceed {_defaultLength} characters.");
        }

        return new OrderName(value);
    }
}