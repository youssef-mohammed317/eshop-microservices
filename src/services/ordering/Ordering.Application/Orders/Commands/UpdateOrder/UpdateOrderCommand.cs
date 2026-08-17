

namespace Ordering.Application.Orders.Commands.UpdateOrder;

// Result returning a boolean indicating success
public record UpdateOrderResult(bool IsSuccess);

// Command containing the updated OrderDto
public record UpdateOrderCommand(OrderDto Order) : ICommand<UpdateOrderResult>;

public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderCommandValidator()
    {
        // 1. Validate the Order Id (Crucial for updates)
        RuleFor(x => x.Order.Id)
            .NotEmpty().WithMessage("Order Id is required.");

        // 2. Top-level property validations
        RuleFor(x => x.Order.OrderName)
            .NotEmpty().WithMessage("Order Name is required.");

        RuleFor(x => x.Order.CustomerId)
            .NotNull().WithMessage("Customer Id is required.")
            .NotEmpty().WithMessage("Customer Id cannot be empty.");

        // 3. Validate that the list is not empty
        RuleFor(x => x.Order.OrderItems)
            .NotEmpty().WithMessage("Order must contain at least one item.");

        // 4. Delegate validation of each item in the list to the child validator
        RuleForEach(x => x.Order.OrderItems)
            .SetValidator(new OrderItemDtoValidator());

        // 5. Delegate validation of complex objects to their respective child validators
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