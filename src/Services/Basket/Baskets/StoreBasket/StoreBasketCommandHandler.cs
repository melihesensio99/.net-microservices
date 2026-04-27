using Basket.API.Data;
using Basket.API.Models;
using Discount.gRPC;
using FluentValidation;
using JasperFx.Events.Daemon;
using Marten;
using MediatR;
using static Discount.gRPC.DiscountProtoService;

namespace Basket.API.Baskets.StoreBasket
{
    public record StoreBasketCommand(ShoppingCart Cart) : IRequest<StoreBasketResult>;
    public record StoreBasketResult(string UserName);
    public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
    {
        public StoreBasketCommandValidator()
        {
            RuleFor(x => x.Cart).NotNull().WithMessage("Sepet boş olamaz!");
            RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("Kullanıcı adı şarttır!");
        }
    }
    public class StoreBasketCommandHandler(IDocumentSession session, IBasketRepository repository, DiscountProtoServiceClient discountProto) : IRequestHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle(StoreBasketCommand request, CancellationToken cancellationToken)
        {
            await ApplyDiscounts(request.Cart, cancellationToken);
            await repository.StoreBasket(request.Cart, cancellationToken);
            return new StoreBasketResult(request.Cart.UserName);
        }

        private async Task ApplyDiscounts(ShoppingCart cart, CancellationToken cancellationToken)
        {
            foreach (var item in cart.Items)
            {
                var cuppon = await discountProto.GetDiscountAsync(new GetDiscountRequest { ProductName = item.ProductName }, cancellationToken: cancellationToken);
                item.Price -= cuppon.Amount;
            }
        }
    }

}
