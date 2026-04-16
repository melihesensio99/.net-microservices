using Carter;
using CatalogAPI.Products.CreateProduct;
using Mapster;
using MediatR;

namespace CatalogAPI.Products.UpdateProduct
{
    public record UpdateProductRequest(Guid id, string Name, string Description, List<string> Category, string ImageFile, decimal Price);
    public record UpdateProductResponse(bool isSuccess);
    public class UpdateProductEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/products", async (UpdateProductRequest request, ISender sender) =>
            {
                var command = request.Adapt<UpdateProductCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<UpdateProductResponse>();
                return Results.Ok(response);

            })
                 .WithName("UpdateProduct")
                .Produces<CreateProductResponse>(StatusCodes.Status200OK)
                .WithSummary("Updates a product")
                .WithDescription("Updates a product with the provided details and returns the updated product's ID.");



        }
    }
}
