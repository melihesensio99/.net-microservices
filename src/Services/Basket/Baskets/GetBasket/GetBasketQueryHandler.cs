using Basket.API.Models;
using Marten;
using MediatR;
using Basket.API.Data;

namespace Basket.API.Baskets.GetBasket;

public record GetBasketQuery(string UserName) : IRequest<GetBasketResult>;
public record GetBasketResult(ShoppingCart Cart);

public class GetBasketQueryHandler(IBasketRepository repository)
    : IRequestHandler<GetBasketQuery, GetBasketResult>
{
    public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
    {
        var basket = await repository.GetBasket(query.UserName, cancellationToken);

        return new GetBasketResult(basket ?? new ShoppingCart(query.UserName));
    }
}
