using Carter;
using Mapster;
using MediatR;

namespace CatalogAPI.Products.DeleteProduct
{
    //public record DeleteProductRequest(Guid Id);
    public record DeleteProductResponse();

    public class DeleteProductEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/products/{id:guid}", async (Guid id, ISender sender) =>
             {
                 var result = await sender.Send(new DeleteProductCommand(id));
                 var response = result.Adapt<DeleteProductResponse>();
                 return Results.Ok(response);   
             })
                  .WithName("DeleteProduct")
                 .Produces(StatusCodes.Status204NoContent)
                 .WithSummary("Deletes a product")
                 .WithDescription("Deletes a specific product by its unique identifier.");
        }
    }
}
