using Carter;
using CatalogAPI.Models;
using Mapster;
using MediatR;

namespace CatalogAPI.Products.GetProductById
{
    //public record GetProductByIdRequest(Guid id);
    public record GetProductByIdResponse(Product Product);
    public class GetProductByIdEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/{id:guid}", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new GetProductByIdQuery(id)); // queryi mediator a gönderiyoruz ve handler tarafından işlenmesini sağlıyoruz
                var response = result.Adapt<GetProductByIdResponse>(); //handler dan gelen sonucu response a adapte ediyoruz
                return Results.Ok(response); //oluşturulan ürünün ID'sini içeren bir URL ve response body ile birlikte 201 Created durum kodu döndürüyoruz

                //requesti al query ya da command neyse ona adapt et ve mediator a gönder handler tarafından işlenmesini sağla handler dan gelen sonucu response a adapte et ve sonuç olarak 200 OK ile birlikte response body döndür


            }).WithName("GetProductById")
                .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
                .WithSummary("Gets a product by ID")
                .WithDescription("Retrieves the details of a specific product by its unique identifier.");
        }
    }
}
