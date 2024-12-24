using VelozientComputers.Api.Configurations;
using VelozientComputers.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperConfig));

// Configure OpenAPI/Swagger
OpenApiConfig.AddOpenApi(builder.Services);

// Get connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Initialize database
DatabaseHelper.InitializeDatabase(connectionString);

// Add database configuration
builder.Services.AddDatabase(connectionString);

// Add dependency injection
builder.Services.AddDependencyInjection();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Velozient Computers API V1");
    c.RoutePrefix = string.Empty;
});

app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();