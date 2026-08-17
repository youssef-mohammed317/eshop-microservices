namespace Ordering.Application.Orders.Commands.CreateOrder;

public record CreateOrderResult(Guid Id);
public record CreateOrderCommand(OrderDto Order) : ICommand<CreateOrderResult>;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        // 1. Top-level property validations
        RuleFor(x => x.Order.OrderName)
            .NotEmpty().WithMessage("Order Name is required.");

        RuleFor(x => x.Order.CustomerId)
            .NotNull().WithMessage("Customer Id is required.")
            .NotEmpty().WithMessage("Customer Id cannot be empty.");

        // 2. Validate that the list is not empty
        RuleFor(x => x.Order.OrderItems)
            .NotEmpty().WithMessage("Order must contain at least one item.");

        // 3. Delegate validation of each item in the list to the child validator
        RuleForEach(x => x.Order.OrderItems)
            .SetValidator(new OrderItemDtoValidator());

        // 4. Delegate validation of complex objects to their respective child validators
        RuleFor(x => x.Order.ShippingAddress)
            .NotNull().WithMessage("Shipping Address is required.")
            .SetValidator(new AddressDtoValidator());

        RuleFor(x => x.Order.BillingAddress)
            .NotNull().WithMessage("Billing Address is required.")
            .SetValidator(new AddressDtoValidator());

        RuleFor(x => x.Order.Payment)
            .NotNull().WithMessage("Payment details are required.")
            .SetValidator(new PaymentDtoValidator());
    }
}

