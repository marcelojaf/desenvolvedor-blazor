using VelozientComputers.Api.Configurations;
using VelozientComputers.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure OpenAPI/Swagger
OpenApiConfig.AddOpenApi(builder.Services);

// Add database configuration
builder.Services.AddDatabase(builder.Configuration.GetConnectionString("DefaultConnection"));

// Add dependency injection
builder.Services.AddDependencyInjection();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();