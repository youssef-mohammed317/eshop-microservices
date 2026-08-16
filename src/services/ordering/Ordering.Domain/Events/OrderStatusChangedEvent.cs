namespace Ordering.Domain.Events;

public record OrderStatusChangedEvent(OrderId Id, OrderStatus OrderStatus) : IDomainEvent;