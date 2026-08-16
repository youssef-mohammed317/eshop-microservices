namespace Ordering.Domain.Models;

public class OrderItem : Entity<OrderItemId>
{
    public OrderId OrderId { get; private set; } = default!;
    public ProductId ProductId { get; private set; } = default!;
    public int Quantity { get; private set; } = default!;
    public decimal Price { get; private set; } = default!;

    // Protected parameterless constructor required by Entity Framework Core
    protected OrderItem() { }

    // Private constructor to prevent direct instantiation
    private OrderItem(OrderItemId id, OrderId orderId, ProductId productId, int quantity, decimal price)
    {
        Id = id;
        OrderId = orderId;
        ProductId = productId;
        Quantity = quantity;
        Price = price;
    }

    /// <summary>
    /// Internal factory method to create a new OrderItem.
    /// It is internal because OrderItems should only be created through the Order Aggregate Root.
    /// </summary>
    internal static OrderItem Create(OrderId orderId, ProductId productId, int quantity, decimal price)
    {
        // Validation rules
        if (quantity <= 0)
            throw new DomainException("Quantity must be greater than zero.");

        if (price < 0)
            throw new DomainException("Price cannot be negative.");

        // Automatically generate a new ID for the new OrderItem
        var id = OrderItemId.Of(Guid.NewGuid());

        return new OrderItem(id, orderId, productId, quantity, price);
    }

    /// <summary>
    /// Used to reconstitute an OrderItem object from the database.
    /// </summary>
    public static OrderItem Of(OrderItemId id, OrderId orderId, ProductId productId, int quantity, decimal price)
    {
        return new OrderItem(id, orderId, productId, quantity, price);
    }
}