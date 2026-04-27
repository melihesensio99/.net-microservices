using Basket.API.Data;
using Basket.API.Models;
using FluentValidation;
using JasperFx.Events.Daemon;
using Marten;
using MediatR;

namespace Basket.API.Baskets.RemoveBasket
{
    public record RemoveBasketCommand(string userName) : IRequest<RemoveBasketResult>;
    public record RemoveBasketResult(bool IsSuccess);
    public class RemoveBasketCommandValidator : AbstractValidator<RemoveBasketCommand>
    {
        public RemoveBasketCommandValidator()
        {
            RuleFor(x => x.userName).NotEmpty().WithMessage("Kullanıcı adı boş olamaz!");
        }
    }

    public class RemoveBasketCommandHandler(IDocumentSession session, IBasketRepository repository) : IRequestHandler<RemoveBasketCommand, RemoveBasketResult>
    {
        public async Task<RemoveBasketResult> Handle(RemoveBasketCommand request, CancellationToken cancellationToken)
        {
            await repository.DeleteBasket(request.userName, cancellationToken);
            return new RemoveBasketResult(true);
        }
    }
}

