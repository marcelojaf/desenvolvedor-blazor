using VelozientComputers.Web.Services;
using VelozientComputers.Web.Services.Interfaces;

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
        // Register HTTP client
        services.AddScoped(sp => new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7126")  // API base URL
        });

        // Register services
        services.AddScoped<IComputerService, ComputerService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAssignmentService, AssignmentService>();

        return services;
    }
}