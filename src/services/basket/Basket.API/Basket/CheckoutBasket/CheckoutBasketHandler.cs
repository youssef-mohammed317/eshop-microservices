namespace Basket.API.Basket.CheckoutBasket;

public record CheckoutBasketCommand(BasketCheckoutDto BasketCheckoutDto) : ICommand<CheckoutBasketResult>;
public record CheckoutBasketResult(bool IsSuccess);

public class CheckoutBasketCommandValidator : AbstractValidator<CheckoutBasketCommand>
{
    public CheckoutBasketCommandValidator()
    {
        RuleFor(x => x.BasketCheckoutDto).NotNull().WithMessage("BasketCheckoutDto can't be null");
        RuleFor(x => x.BasketCheckoutDto.UserName).NotEmpty().WithMessage("UserName is required");
    }
}

public class CheckoutBasketCommandHandler(IBasketRepository repository, IPublishEndpoint publishEndpoint)
    : ICommandHandler<CheckoutBasketCommand, CheckoutBasketResult>
{
    public async Task<CheckoutBasketResult> Handle(CheckoutBasketCommand command, CancellationToken cancellationToken)
    {
        // 1. Get existing basket with total price
        // (Assuming GetBasket throws a NotFoundException if the basket doesn't exist)
        var basket = await repository.GetBasket(command.BasketCheckoutDto.UserName, cancellationToken);

        if (basket == null)
        {
            return new CheckoutBasketResult(false);
        }

        // 2. Map the request DTO to the Integration Event
        var eventMessage = command.BasketCheckoutDto.Adapt<BasketCheckoutEvent>();

        // Set the total price from the basket retrieved from the database (Secure, avoids client manipulation)
        eventMessage.TotalPrice = basket.TotalPrice;

        // 3. Publish the event to RabbitMQ
        await publishEndpoint.Publish(eventMessage, cancellationToken);

        // 4. Delete the basket since the checkout is completed
        await repository.DeleteBasket(command.BasketCheckoutDto.UserName, cancellationToken);

        return new CheckoutBasketResult(true);
    }
}



