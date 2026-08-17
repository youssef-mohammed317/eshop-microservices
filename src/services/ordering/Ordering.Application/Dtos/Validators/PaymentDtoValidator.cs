public class PaymentDtoValidator : AbstractValidator<PaymentDto>
{
    public PaymentDtoValidator()
    {
        RuleFor(x => x.CardName).NotEmpty().WithMessage("Card Name is required.");

        // Ensure card number has exactly 16 digits (adjust based on your business rules)
        RuleFor(x => x.CardNumber)
            .NotEmpty().WithMessage("Card Number is required.")
            .Length(16).WithMessage("Card Number must be 16 characters long.");

        RuleFor(x => x.Expiration).NotEmpty().WithMessage("Expiration is required.");

        // Ensure CVV is exactly 3 digits
        RuleFor(x => x.Cvv)
            .NotEmpty().WithMessage("CVV is required.")
            .Length(3).WithMessage("CVV must be 3 characters long.");

        // Validate that the payment method is within a valid range (Assuming 1 or 2 are valid)
        RuleFor(x => x.PaymentMethod)
            .GreaterThan(0).WithMessage("Payment Method is required.");
    }
}

