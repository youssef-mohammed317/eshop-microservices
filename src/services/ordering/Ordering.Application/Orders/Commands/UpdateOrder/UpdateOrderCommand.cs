

namespace Ordering.Application.Orders.Commands.UpdateOrder;

// Result returning a boolean indicating success
public record UpdateOrderResult(bool IsSuccess);

// Command containing the updated OrderDto
public record UpdateOrderCommand(OrderDto Order) : ICommand<UpdateOrderResult>;

public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderCommandValidator()
    {
        // Ensure the ID is provided for updating
        RuleFor(x => x.Order.Id)
            .NotEmpty().WithMessage("Order Id is required.");

        RuleFor(x => x.Order.OrderName)
            .NotEmpty().WithMessage("Order Name is required.");

        RuleFor(x => x.Order.CustomerId)
            .NotNull().WithMessage("Customer Id is required.");
    }
}