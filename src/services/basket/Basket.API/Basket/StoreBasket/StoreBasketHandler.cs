using Discount.Grpc;

namespace Basket.API.Basket.StoreBasket;

public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;
public record StoreBasketResult(string UserName);

public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
{
    public StoreBasketCommandValidator()
    {
        RuleFor(p => p.Cart).NotNull().WithMessage("Cart can not be null");
        RuleFor(p => p.Cart.UserName).NotEmpty().WithMessage("UserName is required");
    }
}

public class StoreBasketCommandHandler(IBasketRepository _basketRepository,
    DiscountProtoService.DiscountProtoServiceClient _discountProto) : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {
        await DeductDiscount(command, cancellationToken);

        await _basketRepository.StoreBasket(command.Cart, cancellationToken);

        return new(command.Cart.UserName);
    }

    private async Task DeductDiscount(StoreBasketCommand command, CancellationToken cancellationToken)
    {
        foreach (var item in command.Cart.Items)
        {
            var coupon = await _discountProto.GetDiscountAsync(new() { ProductName = item.ProductName }, cancellationToken: cancellationToken);
            item.Price -= coupon.Amount;
        }
    }
}
