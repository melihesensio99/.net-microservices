using BuildingBlocks.CQRS;
using CatalogAPI.Models;
using FluentValidation;
using Marten;
using MediatR;

namespace CatalogAPI.Products.CreateProduct
{
    public record CreateProductCommand(string Name, string Description, List<string> Category, string ImageFile, decimal Price) : ICommand<CreateProductResult>;
    public record CreateProductResult(Guid id);
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Product name is required.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Product description is required.");
            RuleFor(x => x.Category).NotEmpty().WithMessage("At least one category is required.");
            RuleFor(x => x.ImageFile).NotEmpty().WithMessage("Image file is required.");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than zero.");
        }
    }
    public class CreateProductCommandHandler(IDocumentSession session) : IRequestHandler<CreateProductCommand, CreateProductResult>
    {

        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            //var validatrResult = await validator.ValidateAsync(command, cancellationToken);
            //var errors = validatrResult.Errors.Select(e => e.ErrorMessage).ToList();
            //if (errors.Any())
            //    throw new ValidationException(errors.FirstOrDefault());


            var product = new Product
            {
                Category = command.Category
        .Select(c => c.Trim().ToLower())
        .ToList(),
                Description = command.Description,
                ImageFile = command.ImageFile,
                Name = command.Name,
                Price = command.Price,
                CreatedAt = DateTime.UtcNow
            };

            session.Store(product);
            await session.SaveChangesAsync(cancellationToken);  //martenin kendi icinde zaten repository var gereksiz bir sekilde repository pattern kullanmamiza gerek yok

            return new CreateProductResult(product.Id);
        }
    }
}
