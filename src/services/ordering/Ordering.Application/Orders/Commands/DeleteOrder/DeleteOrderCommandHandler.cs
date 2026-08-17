namespace Ordering.Application.Orders.Commands.DeleteOrder;

public class DeleteOrderCommandHandler(IApplicationDbContext dbContext)
    : ICommandHandler<DeleteOrderCommand, DeleteOrderResult>
{
    public async Task<DeleteOrderResult> Handle(DeleteOrderCommand command, CancellationToken cancellationToken)
    {
        var orderId = OrderId.Of(command.OrderId);

        // 1. Find the existing order in the database
        var order = await dbContext.Orders
            .FindAsync([orderId], cancellationToken);

        if (order is null)
        {
            // Throw exception if not found
            throw new OrderNotFoundException(command.OrderId);
        }

        // 2. Remove the order from the database
        dbContext.Orders.Remove(order);

        // 3. Save changes
        await dbContext.SaveChangesAsync(cancellationToken);

        return new DeleteOrderResult(true);
    }
}