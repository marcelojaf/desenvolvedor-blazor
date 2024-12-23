using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VelozientComputers.Core.Entities
{
    /// <summary>
    /// Represents a status assignment for a computer.
    /// </summary>
    [Table("lnk_computer_computer_status")]
    public class ComputerStatusAssignment : BaseEntity
    {
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
        [Required]
        [Column("assign_dt")]
        public DateTime AssignDate { get; set; }

        /// <summary>
        /// Gets or sets the computer associated with this status assignment.
        /// </summary>
        [ForeignKey("ComputerId")]
        public virtual Computer Computer { get; set; }

        /// <summary>
        /// Gets or sets the status associated with this assignment.
        /// </summary>
        [ForeignKey("ComputerStatusId")]
        public virtual ComputerStatus Status { get; set; }
    }
}