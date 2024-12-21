using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VelozientComputers.Core.Entities;

namespace VelozientComputers.Infrastructure.Data.Mappings
{
    /// <summary>
    /// Mapping configuration for the ComputerStatusAssignment entity.
    /// </summary>
    public class ComputerStatusAssignmentMapping : IEntityTypeConfiguration<ComputerStatusAssignment>
    {
        /// <summary>
        /// Configures the entity mapping for ComputerStatusAssignment.
        /// </summary>
        /// <param name="builder">The entity type builder.</param>
        public void Configure(EntityTypeBuilder<ComputerStatusAssignment> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.AssignDate)
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.HasOne(e => e.Computer)
                   .WithMany(c => c.StatusAssignments)
                   .HasForeignKey(e => e.ComputerId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Status)
                   .WithMany(s => s.StatusAssignments)
                   .HasForeignKey(e => e.ComputerStatusId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Add index for queries by computer and status
            builder.HasIndex(e => new { e.ComputerId, e.ComputerStatusId, e.AssignDate });
        }
    }
}