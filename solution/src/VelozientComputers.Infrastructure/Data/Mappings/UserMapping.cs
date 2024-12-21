using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VelozientComputers.Core.Entities;

namespace VelozientComputers.Infrastructure.Data.Mappings
{
    /// <summary>
    /// Mapping configuration for the User entity.
    /// </summary>
    public class UserMapping : IEntityTypeConfiguration<User>
    {
        /// <summary>
        /// Configures the entity mapping for User.
        /// </summary>
        /// <param name="builder">The entity type builder.</param>
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.FirstName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(e => e.LastName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(e => e.EmailAddress)
                   .IsRequired()
                   .HasMaxLength(256);

            builder.Property(e => e.CreateDate)
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

            // Ensure unique email
            builder.HasIndex(e => e.EmailAddress)
                   .IsUnique();

            // Add index for name search
            builder.HasIndex(e => new { e.FirstName, e.LastName });
        }
    }
}