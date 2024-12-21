using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VelozientComputers.Core.Entities;

namespace VelozientComputers.Infrastructure.Data.Mappings
{
    /// <summary>
    /// Mapping configuration for the ComputerManufacturer entity.
    /// </summary>
    public class ComputerManufacturerMapping : IEntityTypeConfiguration<ComputerManufacturer>
    {
        /// <summary>
        /// Configures the entity mapping for ComputerManufacturer.
        /// </summary>
        /// <param name="builder">The entity type builder.</param>
        public void Configure(EntityTypeBuilder<ComputerManufacturer> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasIndex(e => e.Name)
                   .IsUnique();

            builder.Property(e => e.Name)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(e => e.SerialRegex)
                   .HasMaxLength(200);
        }
    }
}