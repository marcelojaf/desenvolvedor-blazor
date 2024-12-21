using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VelozientComputers.Core.Entities
{
    /// <summary>
    /// Represents a status that a computer can have in the system.
    /// </summary>
    [Table("computer_status")]
    public class ComputerStatus
    {
        /// <summary>
        /// Gets or sets the unique identifier for the status.
        /// </summary>
        [Key]
        [Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the localized name of the status.
        /// </summary>
        [Required]
        [Column("localized_name")]
        public string LocalizedName { get; set; }

        /// <summary>
        /// Gets or sets the collection of status assignments.
        /// </summary>
        public ICollection<ComputerStatusAssignment> StatusAssignments { get; set; }
    }
}
