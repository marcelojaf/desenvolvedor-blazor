using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VelozientComputers.Core.Entities;

namespace VelozientComputers.Infrastructure.Data.Mappings
{
    /// <summary>
    /// Mapping configuration for the ComputerStatus entity.
    /// </summary>
    public class ComputerStatusMapping : IEntityTypeConfiguration<ComputerStatus>
    {
        /// <summary>
        /// Configures the entity mapping for ComputerStatus.
        /// </summary>
        /// <param name="builder">The entity type builder.</param>
        public void Configure(EntityTypeBuilder<ComputerStatus> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.LocalizedName)
                   .IsRequired()
                   .HasMaxLength(50);

            // Add unique index for localized name
            builder.HasIndex(e => e.LocalizedName)
                   .IsUnique();
        }
    }
}