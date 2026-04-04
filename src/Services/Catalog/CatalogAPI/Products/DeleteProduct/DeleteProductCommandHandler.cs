using BuildingBlocks.CQRS;
using CatalogAPI.Models;
using Marten;

namespace CatalogAPI.Products.DeleteProduct
{
    public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;
    public record DeleteProductResult(bool Success);
    public class DeleteProductCommandHandler(IDocumentSession session, ILogger<DeleteProductCommandHandler> logger) : ICommandHandler<DeleteProductCommand, DeleteProductResult>
    {
        public async Task<DeleteProductResult> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await session.LoadAsync<Product>(request.Id, cancellationToken);
            if (product == null)
            {
                logger.LogWarning("Product with ID {ProductId} not found for deletion", request.Id);
                return new DeleteProductResult(false);
            }
            session.Delete<Product>(request.Id);
            await session.SaveChangesAsync(cancellationToken);
            return new DeleteProductResult(true);

        }
    }
}
