namespace Ordering.Application.Orders.Commands.CreateOrder;

public record CreateOrderResult(Guid Id);
public record CreateOrderCommand(OrderDto Order) : ICommand<CreateOrderResult>;


public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.Order.OrderName)
            .NotEmpty().WithMessage("Order Name is required.");

        RuleFor(x => x.Order.CustomerId)
            .NotNull().WithMessage("Customer Id is required.");

        RuleFor(x => x.Order.OrderItems)
            .NotEmpty().WithMessage("Order must contain at least one item.");

    }
}
