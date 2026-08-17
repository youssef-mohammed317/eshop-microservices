namespace Ordering.Application.Orders.Commands.DeleteOrder;

// Result returning a boolean indicating success
public record DeleteOrderResult(bool IsSuccess);

// Command taking only the OrderId
public record DeleteOrderCommand(Guid OrderId) : ICommand<DeleteOrderResult>;

public class DeleteOrderCommandValidator : AbstractValidator<DeleteOrderCommand>
{
    public DeleteOrderCommandValidator()
    {
        // Ensure the provided ID is not an empty GUID
        RuleFor(x => x.OrderId)
            .NotEmpty().WithMessage("Order Id is required.");
    }
}
