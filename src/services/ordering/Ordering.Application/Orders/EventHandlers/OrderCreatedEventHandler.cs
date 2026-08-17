using Microsoft.Extensions.Logging;

namespace Ordering.Application.Orders.EventHandlers;

public class OrderCreatedEventHandler(ILogger<OrderCreatedEventHandler> logger)
    : INotificationHandler<OrderCreatedEvent>
{
    public Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
    {
        // 1. Log the event
        logger.LogInformation("Domain Event Handled: {DomainEvent} for Order {OrderId}",
            notification.GetType().Name,
            notification.Order.Id.Value);

        // 2. Here you can add logic like:
        // - Send an order confirmation email to the customer
        // - Publish a message to RabbitMQ/Azure Service Bus for other microservices (Integration Event)
        // - Notify the fulfillment center

        return Task.CompletedTask;
    }
}
public class OrderStatusChangedEventHandler(ILogger<OrderStatusChangedEventHandler> logger)
    : INotificationHandler<OrderStatusChangedEvent>
{
    public Task Handle(OrderStatusChangedEvent notification, CancellationToken cancellationToken)
    {
        // 1. Log the event with the new status
        logger.LogInformation("Domain Event Handled: {DomainEvent} for Order {OrderId}. New Status: {Status}",
            notification.GetType().Name,
            notification.Id.Value, // Assuming the event has the ID
            notification.OrderStatus);    // Assuming the event has the new status

        // 2. Add side-effect logic:
        // - Send an SMS/Email to the user: "Your order is now on the way!"
        // - Inform the accounting microservice if status is 'Paid'

        return Task.CompletedTask;
    }
}