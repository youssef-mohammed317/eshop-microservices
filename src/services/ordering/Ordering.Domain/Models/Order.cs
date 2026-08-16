namespace Ordering.Domain.Models;

public class Order : Aggregate<OrderId> // Aggregate root
{
    private readonly List<OrderItem> _orderItems = new();

    // Expose the list as read-only to prevent external modification
    public IReadOnlyList<OrderItem> OrderItems => _orderItems.AsReadOnly();

    public CustomerId CustomerId { get; private set; } = default!;
    public OrderName OrderName { get; private set; } = default!;
    public Address ShippingAddress { get; private set; } = default!;
    public Address BillingAddress { get; private set; } = default!;
    public Payment Payment { get; private set; } = default!;
    public OrderStatus OrderStatus { get; private set; } = default!;

    // Calculated property
    public decimal TotalPrice
    {
        get => OrderItems.Sum(x => x.Price * x.Quantity);
        private set { }
    }

    // Protected parameterless constructor required by Entity Framework Core
    protected Order() { }

    // Private constructor to prevent direct instantiation
    private Order(
        OrderId id,
        CustomerId customerId,
        OrderName orderName,
        Address shippingAddress,
        Address billingAddress,
        Payment payment,
        OrderStatus orderStatus)
    {
        Id = id;
        CustomerId = customerId;
        OrderName = orderName;
        ShippingAddress = shippingAddress;
        BillingAddress = billingAddress;
        Payment = payment;
        OrderStatus = orderStatus;
    }

    /// <summary>
    /// Creates a new Order instance and initializes the aggregate.
    /// </summary>
    public static Order Create(
        OrderId id,
        CustomerId customerId,
        OrderName orderName,
        Address shippingAddress,
        Address billingAddress,
        Payment payment)
    {
        // Null checks for Value Objects
        if (id == null) throw new DomainException("Order Id cannot be null.");
        if (customerId == null) throw new DomainException("Customer Id cannot be null.");
        if (orderName == null) throw new DomainException("Order Name cannot be null.");
        if (shippingAddress == null) throw new DomainException("Shipping Address cannot be null.");
        if (billingAddress == null) throw new DomainException("Billing Address cannot be null.");
        if (payment == null) throw new DomainException("Payment details cannot be null.");

        // A new order starts with a 'Pending' status
        var order = new Order(
            id,
            customerId,
            orderName,
            shippingAddress,
            billingAddress,
            payment,
            OrderStatus.Pending);

        // Add a domain event
        order.AddDomainEvent(new OrderCreatedEvent(order));

        return order;
    }

    /// <summary>
    /// Updates the order details.
    /// OrderId and CustomerId are excluded because they represent the identity and owner of the order, which shouldn't change.
    /// </summary>
    public void Update(
        OrderName orderName,
        Address shippingAddress,
        Address billingAddress,
        Payment payment,
        OrderStatus orderStatus)
    {
        // Null checks to ensure we aren't replacing valid data with nulls
        if (orderName == null) throw new DomainException("Order Name cannot be null.");
        if (shippingAddress == null) throw new DomainException("Shipping Address cannot be null.");
        if (billingAddress == null) throw new DomainException("Billing Address cannot be null.");
        if (payment == null) throw new DomainException("Payment details cannot be null.");

        // Update the properties
        OrderName = orderName;
        ShippingAddress = shippingAddress;
        BillingAddress = billingAddress;
        Payment = payment;
        OrderStatus = orderStatus;

        // If you are using Domain Events, this is where you would trigger an update event:
        AddDomainEvent(new OrderUpdatedEvent(this));
    }


    /// <summary>
    /// Reconstitutes an existing Order from the database.
    /// </summary>
    public static Order Of(
        OrderId id,
        CustomerId customerId,
        OrderName orderName,
        Address shippingAddress,
        Address billingAddress,
        Payment payment,
        OrderStatus orderStatus)
    {
        return new Order(id, customerId, orderName, shippingAddress, billingAddress, payment, orderStatus);
    }



    /// <summary>
    /// Adds a new product (OrderItem) to the order. 
    /// This is the only way to add items, ensuring the Aggregate Root controls the state.
    /// </summary>
    public void Add(ProductId productId, int quantity, decimal price)
    {
        // You could also check if the item already exists in the list and just increase the quantity
        var existingItem = _orderItems.FirstOrDefault(x => x.ProductId == productId);

        if (existingItem != null)
        {
            // If the item exists, you might want to increase quantity 
            // (requires an internal method on OrderItem to UpdateQuantity)
            // Or throw an exception depending on your business rules
            throw new DomainException("Product already exists in the order.");
        }

        var orderItem = OrderItem.Create(Id, productId, quantity, price);
        _orderItems.Add(orderItem);
    }

    /// <summary>
    /// Removes a product (OrderItem) from the order.
    /// </summary>
    public void Remove(ProductId productId)
    {
        var orderItem = _orderItems.FirstOrDefault(x => x.ProductId == productId);

        if (orderItem != null)
        {
            _orderItems.Remove(orderItem);
        }
    }

    /// <summary>
    /// Changes the status of the order.
    /// </summary>
    public void ChangeStatus(OrderStatus newStatus)
    {
        // You can add state machine validation here (e.g., cannot change from Cancelled to Shipped)
        if (OrderStatus == OrderStatus.Completed || OrderStatus == OrderStatus.Cancelled)
        {
            throw new DomainException($"Cannot change status from {OrderStatus} to {newStatus}. Order is already finalized.");
        }

        OrderStatus = newStatus;

        // Add a domain event
        AddDomainEvent(new OrderStatusChangedEvent(Id, newStatus));
    }
}