using BuildingBlocks.CQRS;
using CatalogAPI.Exceptions;
using CatalogAPI.Models;
using FluentValidation;
using Marten;
using MediatR;

namespace CatalogAPI.Products.UpdateProduct
{
    public record UpdateProductCommand(Guid id, string Name, string Description, List<string> Category, string ImageFile, decimal Price) : ICommand<UpdateProductResult>;
    public record UpdateProductResult(bool isSuccess);

    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.id).NotEmpty().WithMessage("Product ID is required.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Product name is required.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Product description is required.");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than zero.");
            RuleFor(x => x.Category).NotEmpty().WithMessage("At least one category is required.");
        }
    }


    public class UpdateProductCommandHandler(IDocumentSession session) : IRequestHandler<UpdateProductCommand, UpdateProductResult>
    {
        public async Task<UpdateProductResult> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await session.LoadAsync<Product>(request.id, cancellationToken);
            if (product == null)
            {

                throw new ProductNotFoundException(request.id);
            }
            product.Name = request.Name;
            product.Description = request.Description;
            product.ImageFile = request.ImageFile;
            product.Price = request.Price;
            product.Category = request.Category;
            product.UpdatedAt = DateTime.UtcNow;

            session.Update(product);
            await session.SaveChangesAsync(cancellationToken);

            return new UpdateProductResult(true);
        }
    }
}
