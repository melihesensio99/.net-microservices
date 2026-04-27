using Basket.API.Data;
using BuildingBlocks.Behaviors;
using BuildingBlocks.Exceptions.Handler;
using Carter;
using FluentValidation;
using Marten;

namespace Basket.API;

public static class BasketServiceRegister
{
    public static IServiceCollection AddBasketServices(this IServiceCollection services, IConfiguration configuration)
    {
        // MediatR & Behaviors
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(BasketServiceRegister).Assembly);
            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

    
        services.AddMarten(config =>
        {
            config.Connection(configuration.GetConnectionString("Database")!);
            config.Schema.For<Models.ShoppingCart>().Identity(x => x.UserName); // UserName'ı ID olarak kullanmak için
        }).UseLightweightSessions();

      
        services.AddCarter(configurator: config => 
        {
            config.WithModule<Baskets.StoreBasket.StoreBasketEndpoints>();
            config.WithModule<Baskets.GetBasket.GetBasketEndpoint>();
            config.WithModule<Baskets.RemoveBasket.RemoveBasketEndpoint>();
        });

        services.AddValidatorsFromAssembly(typeof(BasketServiceRegister).Assembly);
        services.AddExceptionHandler<CustomExceptionHandler>();
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis");
        });

        services.AddHealthChecks()
            .AddNpgSql(configuration.GetConnectionString("Database")!)
            .AddRedis(configuration.GetConnectionString("Redis")!);

        services.AddScoped<IBasketRepository, BasketRepository>();
        services.Decorate<IBasketRepository, CachedBasketRepository>();

        services.AddGrpcClient<Discount.gRPC.DiscountProtoService.DiscountProtoServiceClient>(options =>
        {
            options.Address = new Uri(configuration["GrpcSettings:DiscountUrl"]!);
        });

        return services;
    }
}
