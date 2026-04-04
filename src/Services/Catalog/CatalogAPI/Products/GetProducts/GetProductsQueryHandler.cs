using BuildingBlocks.CQRS;
using CatalogAPI.Exceptions;
using CatalogAPI.Models;
using Marten;

namespace CatalogAPI.Products.GetProduct
{
    public record GetProductsQuery() : IQuery<GetProductsResult>;
    public record GetProductsResult(IEnumerable<Product> Products);
    public class GetProductsQueryHandler(IDocumentSession session, ILogger<GetProductsQueryHandler> logger) : IQueryHandler<GetProductsQuery, GetProductsResult>
    {
        public async Task<GetProductsResult> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling GetProductsQuery");
            var products = await session.Query<Product>().ToListAsync(cancellationToken); //martenin kendi icinde zaten repository var gereksiz bir sekilde repository pattern kullanmamiza gerek yok

            if (products == null || !products.Any())
            {
                logger.LogWarning("No products found");
                throw new ProductNotFoundException();
            }
            return new GetProductsResult(products);

        }
    }
}
