using CatalogAPI.Models;
using Marten;
using Marten.Schema;
using System.Collections.Generic;

namespace CatalogAPI.Data
{
    public class CatalogInitialData : IInitialData
    {
        public async Task Populate(IDocumentStore store, CancellationToken cancellation)
        {
            await using var session = store.LightweightSession();

            if (await session.Query<Product>().AnyAsync())
                return;

            session.Store<Product>(GetDeneme());
            await session.SaveChangesAsync();
        }
        private static IEnumerable<Product> GetDeneme() => new List<Product>
        {
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Product 1",
                Description = "Description for Product 1",
                Category = new List<string> { "Category A", "Category B" },
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                ImageFile = "product1.jpg",
                Price = 19.99m
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Product 2",
                Description = "Description for Product 1",
                Category = new List<string> { "Category A", "Category B" },
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                ImageFile = "product1.jpg",
                Price = 19.99m
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Product 3",
                Description = "Description for Product 1",
                Category = new List<string> { "Category A", "Category B" },
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                ImageFile = "product1.jpg",
                Price = 19.99m
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Product 4",
                Description = "Description for Product 1",
                Category = new List<string> { "Category A", "Category B" },
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                ImageFile = "product1.jpg",
                Price = 19.99m
            }
        };

    }
}