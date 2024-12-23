using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VelozientComputers.Core.Interfaces.Service;
using VelozientComputers.Infrastructure.Services;

namespace VelozientComputers.Infrastructure.Data
{
    /// <summary>
    /// Provides extension methods for configuring the database and related services.
    /// </summary>
    public static class DatabaseConfiguration
    {
        /// <summary>
        /// Adds the database context and related services to the specified IServiceCollection.
        /// </summary>
        /// <param name="services">The IServiceCollection to add services to.</param>
        /// <param name="connectionString">The connection string for the SQLite database.</param>
        /// <returns>The IServiceCollection for method chaining.</returns>
        public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(connectionString, sqliteOptions =>
                {
                    sqliteOptions.MigrationsAssembly("VelozientComputers.Infrastructure");
                }));

            // Register the serial number validation service
            services.AddScoped<ISerialNumberValidationService, SerialNumberValidationService>();

            return services;
        }
    }
}