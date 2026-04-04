using Carter;
using CatalogAPI.Products.CreateProduct;
using CatalogAPI.Products.DeleteProduct;
using CatalogAPI.Products.GetProduct;
using CatalogAPI.Products.GetProductByCategory;
using CatalogAPI.Products.GetProductById;
using CatalogAPI.Products.GetProducts;
using Marten;

namespace CatalogAPI
{
    public static class CatalogServiceRegister
    {
        public static IServiceCollection AddCatalogServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CatalogServiceRegister).Assembly));
            services.AddCarter(configurator: config =>
            {
                config.WithModule<CreateProductEndPoint>();
                config.WithModule<GetProductByIdEndPoint>();
                config.WithModule<GetProductsEndpoint>();
                config.WithModule<GetProductsByCategoryEndPoint>();
                config.WithModule<DeleteProductEndPoint>();
            });

            services.AddMarten(config => config.Connection(configuration.GetConnectionString("Database")))
                .UseLightweightSessions(); //Nesneyi veritabanından çeker ve işi biter. Takip mekanizmasını kapattığı için RAM ve CPU kullanımı düşer.
            return services;
        }
    }
}
