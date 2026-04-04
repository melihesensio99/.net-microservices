using BuildingBlocks.CQRS;
using CatalogAPI.Exceptions;
using CatalogAPI.Models;
using Marten;

namespace CatalogAPI.Products.GetProductById
{
    public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;
    public record GetProductByIdResult(Product Product);
    public class GetProductByIdQueryHandler(IDocumentSession session, ILogger<GetProductByIdQueryHandler> logger) : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
    {
        public async Task<GetProductByIdResult> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await session.LoadAsync<Product>(request.Id, cancellationToken);
            //martenin kendi icinde zaten repository var gereksiz bir sekilde repository pattern kullanmamiza gerek yok
            if (product == null)
            {
                logger.LogWarning("Product with ID {ProductId} not found", request.Id);
                throw new ProductNotFoundException(request.Id);
            }

            return new GetProductByIdResult(product);
        }
    }
}
