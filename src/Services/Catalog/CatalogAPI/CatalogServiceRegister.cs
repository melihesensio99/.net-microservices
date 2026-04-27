using BuildingBlocks.Behaviors;
using BuildingBlocks.Exceptions.Handler;
using Carter;
using CatalogAPI.Data;
using CatalogAPI.Products.CreateProduct;
using CatalogAPI.Products.DeleteProduct;
using CatalogAPI.Products.GetProduct;
using CatalogAPI.Products.GetProductByCategory;
using CatalogAPI.Products.GetProductById;
using CatalogAPI.Products.GetProducts;
using CatalogAPI.Products.UpdateProduct;
using FluentValidation;
using Marten;

namespace CatalogAPI
{
    public static class CatalogServiceRegister
    {
        public static IServiceCollection AddCatalogServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(CatalogServiceRegister).Assembly);
                cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });
            services.AddValidatorsFromAssembly(typeof(CatalogServiceRegister).Assembly);

            services.AddCarter(configurator: config =>
                {
                    config.WithModule<CreateProductEndPoint>();
                    config.WithModule<GetProductByIdEndPoint>();
                    config.WithModule<GetProductsEndpoint>();
                    config.WithModule<GetProductsByCategoryEndPoint>();
                    config.WithModule<DeleteProductEndPoint>();
                    config.WithModule<UpdateProductEndPoint>();
                });
            services.AddExceptionHandler<CustomExceptionHandler>();
            services.AddMarten(config => config.Connection(configuration.GetConnectionString("Database")!))
                .UseLightweightSessions(); //Nesneyi veritabanından çeker ve işi biter. Takip mekanizmasını kapattığı için RAM ve CPU kullanımı düşer.
            var serviceProvider = services.BuildServiceProvider();
            var env = serviceProvider.GetRequiredService<IWebHostEnvironment>();
            if (env.IsDevelopment())
            {
                services.InitializeMartenWith<CatalogInitialData>();
            }

            services.AddHealthChecks().AddNpgSql(configuration.GetConnectionString("Database")!);
            return services;


        }
    }
}
