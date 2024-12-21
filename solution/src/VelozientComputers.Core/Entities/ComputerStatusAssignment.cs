using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VelozientComputers.Core.Entities
{
    /// <summary>
    /// Represents a status assignment for a computer.
    /// </summary>
    [Table("lnk_computer_computer_status")]
    public class ComputerStatusAssignment
    {
        /// <summary>
        /// Gets or sets the unique identifier for the status assignment.
        /// </summary>
        [Key]
        [Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the computer ID for this assignment.
        /// </summary>
        [Required]
        [Column("computer_id")]
        public int ComputerId { get; set; }

        /// <summary>
        /// Gets or sets the computer status ID for this assignment.
        /// </summary>
        [Required]
        [Column("computer_status_id")]
        public int ComputerStatusId { get; set; }

        /// <summary>
        /// Gets or sets the date when this status was assigned.
        /// </summary>
        [Column("assign_dt")]
        public DateTime AssignDate { get; set; }

        /// <summary>
        /// Gets or sets the computer associated with this status assignment.
        /// </summary>
        [ForeignKey("ComputerId")]
        public Computer Computer { get; set; }

        /// <summary>
        /// Gets or sets the status associated with this assignment.
        /// </summary>
        [ForeignKey("ComputerStatusId")]
        public ComputerStatus Status { get; set; }
    }
}
