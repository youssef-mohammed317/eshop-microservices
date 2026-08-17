using Microsoft.EntityFrameworkCore;

namespace Ordering.Application.Orders.Queries.GetOrders;

public record GetOrdersResult(PaginatedResult<OrderDto> Orders);

public record GetOrdersQuery(PaginationRequest PaginationRequest) : IQuery<GetOrdersResult>;

public class GetOrdersHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetOrdersQuery, GetOrdersResult>
{
    public async Task<GetOrdersResult> Handle(GetOrdersQuery query, CancellationToken cancellationToken)
    {
        var pageIndex = query.PaginationRequest.PageIndex;
        var pageSize = query.PaginationRequest.PageSize;

        // 1. Get the total count of orders for pagination metadata
        var totalCount = await dbContext.Orders.LongCountAsync(cancellationToken);

        // 2. Fetch the paginated orders
        var orders = await dbContext.Orders
            .Include(o => o.OrderItems)
            .AsNoTracking()
            .OrderBy(o => o.OrderName)
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        // 3. Map to DTOs
        var orderDtos = orders.ToOrderDtoList();

        // 4. Wrap inside the PaginatedResult class
        var paginatedResult = new PaginatedResult<OrderDto>(
            pageIndex,
            pageSize,
            totalCount,
            orderDtos);

        return new GetOrdersResult(paginatedResult);
    }
}