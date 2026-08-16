namespace Ordering.Domain.Events;

public record OrderStatusChangedEvent(Order Id, OrderStatus OrderStatus) : IDomainEvent;