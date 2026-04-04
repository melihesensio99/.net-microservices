using BuildingBlocks.CQRS;
using CatalogAPI.Models;
using Marten;
using MediatR;
using System.Windows.Input;

namespace CatalogAPI.Products.CreateProduct
{
    public record CreateProductCommand(string Name, string Description, List<string> Category, string ImageFile, decimal Price) : ICommand<CreateProductResult>;
    public record CreateProductResult(Guid id);
    public class     CreateProductCommonadHandler(IDocumentSession session) : IRequestHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Category = command.Category
        .Select(c => c.Trim().ToLower())
        .ToList(),
                Description = command.Description,
                ImageFile = command.ImageFile,
                Name = command.Name,
                Price = command.Price
            };
            session.Store(product);
            await session.SaveChangesAsync(cancellationToken);  //martenin kendi icinde zaten repository var gereksiz bir sekilde repository pattern kullanmamiza gerek yok

            return new CreateProductResult(product.Id);
        }
    }
}
