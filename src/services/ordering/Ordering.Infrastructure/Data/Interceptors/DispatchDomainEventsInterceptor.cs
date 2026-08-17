using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Ordering.Infrastructure.Data.Interceptors;

// We inject IMediator into the interceptor to publish the events
public class DispatchDomainEventsInterceptor(IMediator mediator) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        DispatchDomainEvents(eventData.Context).GetAwaiter().GetResult();
        return base.SavingChanges(eventData, result);
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        await DispatchDomainEvents(eventData.Context);
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private async Task DispatchDomainEvents(DbContext? context)
    {
        if (context == null) return;

        // 1. Get all aggregates from the ChangeTracker that have pending domain events
        // Note: Change 'IAggregate' to match your base interface/class if it's named differently
        var aggregates = context.ChangeTracker
            .Entries<IAggregate>()
            .Where(a => a.Entity.DomainEvents != null && a.Entity.DomainEvents.Any())
            .Select(a => a.Entity)
            .ToList();

        // 2. Extract all the domain events into a single list
        var domainEvents = aggregates
            .SelectMany(a => a.DomainEvents)
            .ToList();

        // 3. Clear the events from the entities so they don't get dispatched twice
        aggregates.ForEach(a => a.ClearDomainEvents());

        // 4. Publish each event using MediatR
        foreach (var domainEvent in domainEvents)
        {
            await mediator.Publish(domainEvent);
        }
    }
}