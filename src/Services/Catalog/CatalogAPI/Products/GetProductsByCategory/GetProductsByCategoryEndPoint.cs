using Carter;
using CatalogAPI.Models;
using Mapster;
using MediatR;

namespace CatalogAPI.Products.GetProductByCategory
{
    //public record GetProductsByCategoryRequest(string Category);
    public record GetProductsByCategoryResponse(IEnumerable<Product> Products);
    public class GetProductsByCategoryEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/category/{category}", async (string category, ISender sender) =>
            {
                var result = await sender.Send(new GetProductByCategoryQuery(category));
                var response = result.Adapt<GetProductsByCategoryResponse>();
                return Results.Ok(response);
            }).WithName("GetProductsByCategory")
                .Produces<GetProductsByCategoryResponse>(StatusCodes.Status200OK)
                .WithSummary("Gets products by category")
                .WithDescription("Retrieves a list of products that belong to the specified category.");
        }
    }
}
