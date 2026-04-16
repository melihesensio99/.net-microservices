using BuildingBlocks.CQRS;
using CatalogAPI.Exceptions;
using CatalogAPI.Models;
using Marten;
using Marten.Pagination;

namespace CatalogAPI.Products.GetProduct
{
    public record GetProductsQuery(int? pageNumber = 1, int? pageSize = 10) : IQuery<GetProductsResult>;
    public record GetProductsResult(IEnumerable<Product> Products);
    public class GetProductsQueryHandler(IDocumentSession session) : IQueryHandler<GetProductsQuery, GetProductsResult>
    {
        public async Task<GetProductsResult> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {

            var products = await session.Query<Product>().ToPagedListAsync(request.pageNumber ?? 1, request.pageSize ?? 5, cancellationToken); //martenin kendi icinde zaten repository var gereksiz bir sekilde repository pattern kullanmamiza gerek yok

            return new GetProductsResult(products);

        }
    }
}
