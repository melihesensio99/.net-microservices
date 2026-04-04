using BuildingBlocks.CQRS;
using CatalogAPI.Models;
using Marten;
using Marten.Linq.MatchesSql;

namespace CatalogAPI.Products.GetProductByCategory
{
    public record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategoryResult>;
    public record GetProductByCategoryResult(IEnumerable<Product> Products);
    public class GetProductsByCategoryQueryHandler(IDocumentSession session, ILogger<GetProductsByCategoryQueryHandler> logger) : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
    {
        public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery request, CancellationToken cancellationToken)
        {
            var searchTerm = request.Category.Trim().ToLower();
            var products = await session.Query<Product>()
                .Where(p => p.Category.Contains(searchTerm))
                .ToListAsync(cancellationToken);

            if (products == null || !products.Any())
            {
                logger.LogWarning("No products found for category {Category}", products);
                return new GetProductByCategoryResult(Enumerable.Empty<Product>());
            }
            return new GetProductByCategoryResult(products);
        }
    }
}
