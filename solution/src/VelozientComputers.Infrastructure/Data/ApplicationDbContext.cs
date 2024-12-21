using Microsoft.EntityFrameworkCore;
using System.Reflection;
using VelozientComputers.Core.Entities;

namespace VelozientComputers.Infrastructure.Data
{
    /// <summary>
    /// Represents the database context for the Velozient Computers inventory management system.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the ApplicationDbContext.
        /// </summary>
        /// <param name="options">The options to be used by the DbContext.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the DbSet of ComputerManufacturers.
        /// </summary>
        public DbSet<ComputerManufacturer> ComputerManufacturers { get; set; }

        /// <summary>
        /// Gets or sets the DbSet of Computers.
        /// </summary>
        public DbSet<Computer> Computers { get; set; }

        /// <summary>
        /// Gets or sets the DbSet of ComputerStatuses.
        /// </summary>
        public DbSet<ComputerStatus> ComputerStatuses { get; set; }

        /// <summary>
        /// Gets or sets the DbSet of Users.
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Gets or sets the DbSet of ComputerStatusAssignments.
        /// </summary>
        public DbSet<ComputerStatusAssignment> ComputerStatusAssignments { get; set; }

        /// <summary>
        /// Gets or sets the DbSet of ComputerUserAssignments.
        /// </summary>
        public DbSet<ComputerUserAssignment> ComputerUserAssignments { get; set; }

        /// <summary>
        /// Configures the model that was discovered by convention from the entity types.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply entity mappings from the current assembly
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}