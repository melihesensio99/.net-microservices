using Basket.API;
using Carter;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// --- Servisleri Kaydet ---
builder.Services.AddBasketServices(builder.Configuration);

var app = builder.Build();

// --- Pipeline Yapılandırması ---
app.MapCarter();

app.UseExceptionHandler(options => { });

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
