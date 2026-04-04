using Carter;
using CatalogAPI;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCatalogServices(builder.Configuration);

//Add services to the container.
var app = builder.Build();

// Configure HTTP request pipeline.

app.MapCarter();

app.Run();
