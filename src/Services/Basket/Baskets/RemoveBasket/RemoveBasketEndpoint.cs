using Carter;
using MediatR;

namespace Basket.API.Baskets.RemoveBasket
{
    public record RemoveBasketRequest(string UserName);
    public record RemoveBasketResponse(bool IsSuccess);
    public class RemoveBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
           app.MapDelete("/basket/{userName}", async (string userName, ISender sender) =>
            {
                var result = await sender.Send(new RemoveBasketCommand(userName));
                return Results.Ok(result);
            }).WithName("RemoveBasket")
                .Produces<bool>(StatusCodes.Status200OK)
                .WithSummary("Removes the shopping cart for a specific user")
                .WithDescription("Deletes the shopping cart associated with the specified user and returns a boolean indicating success.");
        }
    }
}
