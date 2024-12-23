using VelozientComputers.Core.Interfaces.Repository;
using VelozientComputers.Core.Interfaces.Service;
using VelozientComputers.Core.Services;
using VelozientComputers.Infrastructure.Repository;

namespace VelozientComputers.Api.Configurations
{
    /// <summary>
    /// Configuration for dependency injection services.
    /// </summary>
    public static class DependencyInjectionConfig
    {
        /// <summary>
        /// Registers all services in the dependency injection container.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns>The service collection for chaining.</returns>
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            // Register repositories
            services.AddScoped<IComputerRepository, ComputerRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IManufacturerRepository, ManufacturerRepository>();
            services.AddScoped<IStatusRepository, StatusRepository>();

            // Register services
            services.AddScoped<IComputerService, ComputerService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAssignmentService, AssignmentService>();
            services.AddScoped<ISerialNumberValidationService, SerialNumberValidationService>();

            // Register AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            return services;
        }
    }
}