using Basket.API.Models;
namespace Basket.API.Basket.GetBasket;

public record GetBasketQuery(string UserName) : IQuery<GetBasketResult>;
public record GetBasketResult(ShoppingCart Cart);

public class GetBasketQueryValidator : AbstractValidator<GetBasketQuery>
{
    public GetBasketQueryValidator()
    {
        RuleFor(p => p.UserName).NotEmpty().WithMessage("UserName is required");
    }
}

public class GetBasketQueryHandler(IBasketRepository _basketRepository) : IQueryHandler<GetBasketQuery, GetBasketResult>
{
    public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
    {
        var basket = await _basketRepository.GetBasket(query.UserName, cancellationToken);

        return new(basket);
    }
}
