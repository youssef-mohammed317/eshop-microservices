using Microsoft.EntityFrameworkCore;

namespace Ordering.Application.Orders.Queries.GetOrdersByName;

public class GetOrdersByNameHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetOrdersByNameQuery, GetOrdersByNameResult>
{
    public async Task<GetOrdersByNameResult> Handle(GetOrdersByNameQuery query, CancellationToken cancellationToken)
    {
        // Get orders where the order name contains the requested string
        // AsNoTracking() is used for read-only queries to improve performance
        var orders = await dbContext.Orders
            .Include(o => o.OrderItems) // MUST include related entities for DTO mapping
            .AsNoTracking()
            .Where(o => o.OrderName.Value.Contains(query.Name))
            .OrderBy(o => o.OrderName.Value)
            .ToListAsync(cancellationToken);

        // Convert Domain Entities to DTOs using our extension method
        return new GetOrdersByNameResult(orders.ToOrderDtoList());
    }
}