using Microsoft.OpenApi.Models;

namespace VelozientComputers.Api.Configurations
{
    /// <summary>
    /// Configuration for OpenAPI/Swagger documentation.
    /// </summary>
    public static class OpenApiConfig
    {
        /// <summary>
        /// Adds OpenAPI/Swagger configuration to the service collection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns>The service collection for chaining.</returns>
        public static IServiceCollection AddOpenApi(this IServiceCollection services)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Velozient Computers API",
                    Version = "v1",
                    Description = "API for managing computer inventory and assignments",
                    Contact = new OpenApiContact
                    {
                        Name = "Velozient",
                        Email = "support@velozient.com"
                    }
                });

                // Include XML comments
                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            return services;
        }

        /// <summary>
        /// Maps OpenAPI/Swagger endpoints.
        /// </summary>
        /// <param name="app">The web application.</param>
        public static void MapOpenApi(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Velozient Computers API v1");
                c.RoutePrefix = string.Empty; // Serve the Swagger UI at the app's root
            });
        }
    }
}