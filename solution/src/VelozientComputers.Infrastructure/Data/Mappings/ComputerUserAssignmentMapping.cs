using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VelozientComputers.Core.Entities;

namespace VelozientComputers.Infrastructure.Data.Mappings
{
    /// <summary>
    /// Mapping configuration for the ComputerUserAssignment entity.
    /// </summary>
    public class ComputerUserAssignmentMapping : IEntityTypeConfiguration<ComputerUserAssignment>
    {
        /// <summary>
        /// Configures the entity mapping for ComputerUserAssignment.
        /// </summary>
        /// <param name="builder">The entity type builder.</param>
        public void Configure(EntityTypeBuilder<ComputerUserAssignment> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.AssignStartDate)
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.HasOne(e => e.Computer)
                   .WithMany(c => c.UserAssignments)
                   .HasForeignKey(e => e.ComputerId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.User)
                   .WithMany(u => u.ComputerAssignments)
                   .HasForeignKey(e => e.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Add index for active assignments (SQLite compatible)
            builder.HasIndex(e => new { e.ComputerId, e.AssignEndDate });

            // Add index for user assignments
            builder.HasIndex(e => new { e.UserId, e.AssignStartDate });
        }
    }
}