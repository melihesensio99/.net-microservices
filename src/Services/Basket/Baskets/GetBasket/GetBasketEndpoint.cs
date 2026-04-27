using Basket.API.Models;
using Carter;
using Mapster;
using MediatR;

namespace Basket.API.Baskets.GetBasket
{
    public record GetBasketRequest(string UserName);
    public record GetBasketResponse(ShoppingCart Cart);

    public class GetBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/basket/{userName}", async (string userName, ISender sender) =>
            {
                var result = await sender.Send(new GetBasketQuery(userName));
                var response = result.Adapt<GetBasketResponse>();
                return Results.Ok(response);
            }).WithName("GetBasket")
                .Produces<GetBasketResponse>(StatusCodes.Status200OK)
                .WithSummary("Gets the shopping cart for a specific user")
                .WithDescription("Retrieves the shopping cart details for the specified user, including items and total price.");
        }
    }
}
