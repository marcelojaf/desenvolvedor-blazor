using VelozientComputers.Web.Services;
using VelozientComputers.Web.Services.Interfaces;
using ISerialNumberValidationService = VelozientComputers.Core.Interfaces.Service.ISerialNumberValidationService;

namespace VelozientComputers.Web.Configurations;

/// <summary>
/// Configuration for dependency injection services
/// </summary>
public static class DependencyInjectionConfig
{
    /// <summary>
    /// Registers all services in the dependency injection container
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
    {
        // Register HTTP client with base URL
        services.AddScoped(sp => new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7126")
        });

        // Register domain services
        services.AddScoped<IComputerService, ComputerService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAssignmentService, AssignmentService>();

        // Register validation services
        services.AddScoped<ISerialNumberValidationService, ClientSerialNumberValidationService>();

        return services;
    }
}