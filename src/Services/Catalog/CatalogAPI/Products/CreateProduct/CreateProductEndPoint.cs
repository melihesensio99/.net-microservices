using Carter;
using Mapster;
using MediatR;

namespace CatalogAPI.Products.CreateProduct
{
    public record CreateProductRequest(string Name, string Description, List<string> Category, string ImageFile, decimal Price);
    public record CreateProductResponse(Guid Id);
    public class CreateProductEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/products", async (CreateProductRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreateProductCommand>(); //disardan gelen requesti bizim comm imize adapte ediyoruz
                var result = await sender.Send(command); // commandi mediator a gönderiyoruz ve handler tarafından işlenmesini sağlıyoruz
                var response = result.Adapt<CreateProductResponse>(); //handler dan gelen sonucu response a adapte ediyoruz

                return Results.Created($"/products/{response.Id}", response); //oluşturulan ürünün ID'sini içeren bir URL ve response body ile birlikte 201 Created durum kodu döndürüyoruz
            })
                 .WithName("CreateProduct")
                .Produces<CreateProductResponse>(StatusCodes.Status201Created)
                .WithSummary("Creates a new product")
                .WithDescription("Creates a new product with the provided details and returns the created product's ID.");
        }
    }
}
