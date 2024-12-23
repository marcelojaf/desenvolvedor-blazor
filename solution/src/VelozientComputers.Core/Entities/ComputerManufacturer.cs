using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VelozientComputers.Core.Entities
{
    /// <summary>
    /// Represents a computer manufacturer with its specific serial number validation pattern.
    /// </summary>
    [Table("computer_manufacturer")]
    public class ComputerManufacturer : BaseEntity
    {
        /// <summary>
        /// Gets or sets the unique identifier for the manufacturer.
        /// </summary>
        [Key]
        [Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the manufacturer.
        /// </summary>
        [Required]
        [Column("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the regular expression pattern for validating serial numbers.
        /// </summary>
        [Column("serial_regex")]
        public string SerialRegex { get; set; }

        /// <summary>
        /// Gets or sets the collection of computers from this manufacturer.
        /// </summary>
        public ICollection<Computer> Computers { get; set; }
    }
}
