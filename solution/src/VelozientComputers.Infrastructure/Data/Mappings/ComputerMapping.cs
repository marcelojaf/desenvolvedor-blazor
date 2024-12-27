using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VelozientComputers.Core.Entities;

namespace VelozientComputers.Infrastructure.Data.Mappings
{
    /// <summary>
    /// Mapping configuration for the Computer entity.
    /// </summary>
    public class ComputerMapping : IEntityTypeConfiguration<Computer>
    {
        /// <summary>
        /// Configures the entity mapping for Computer.
        /// </summary>
        /// <param name="builder">The entity type builder.</param>
        public void Configure(EntityTypeBuilder<Computer> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.SerialNumber)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(e => e.Specifications)
                   .HasMaxLength(2000);

            builder.Property(e => e.ImageUrl)
                   .HasMaxLength(2000);

            builder.Property(e => e.CreateDate)
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

            // Configure relationship with manufacturer
            builder.HasOne(e => e.Manufacturer)
                   .WithMany(m => m.Computers)
                   .HasForeignKey(e => e.ComputerManufacturerId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Ensure unique serial number
            builder.HasIndex(e => e.SerialNumber)
                   .IsUnique();

            // Add index for warranty expiration date
            builder.HasIndex(e => e.WarrantyExpirationDate);
        }
    }
}