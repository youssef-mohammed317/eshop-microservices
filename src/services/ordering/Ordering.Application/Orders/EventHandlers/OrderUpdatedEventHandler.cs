using Microsoft.Extensions.Logging;

namespace Ordering.Application.Orders.EventHandlers;

public class OrderUpdatedEventHandler(ILogger<OrderUpdatedEventHandler> logger)
    : INotificationHandler<OrderUpdatedEvent>
{
    public Task Handle(OrderUpdatedEvent notification, CancellationToken cancellationToken)
    {
        // 1. Log the event
        logger.LogInformation("Domain Event Handled: {DomainEvent} for Order {OrderId}",
            notification.GetType().Name,
            notification.Order.Id.Value);

        // 2. Add side-effect logic:
        // - Send an email letting the customer know their order details were updated.

        return Task.CompletedTask;
    }
}
