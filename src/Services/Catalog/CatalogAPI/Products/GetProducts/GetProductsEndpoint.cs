using Carter;
using CatalogAPI.Models;
using CatalogAPI.Products.GetProduct;
using Mapster;
using Marten;
using MediatR;

namespace CatalogAPI.Products.GetProducts
{
    //public record GetProductsRequest();
    public record GetProductsResponse(IEnumerable<Product> products);
    public class GetProductsEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products", async (ISender sender) =>
            {

                var result = await sender.Send(new GetProductsQuery()); // queryi mediator a gönderiyoruz ve handler tarafından işlenmesini sağlıyoruz
                var response = result.Adapt<GetProductsResponse>(); //handler dan gelen sonucu response a adapte ediyoruz
                return Results.Ok(response); //oluşturulan ürünün ID'sini içeren bir URL ve response body ile birlikte 201 Created durum kodu döndürüyoruz

            }).WithName("GetProducts")
                .Produces<GetProductsResponse>(StatusCodes.Status200OK)
                .WithSummary("Gets all products")
                .WithDescription("Retrieves a list of all products available in the catalog.");






        }
    }
}
