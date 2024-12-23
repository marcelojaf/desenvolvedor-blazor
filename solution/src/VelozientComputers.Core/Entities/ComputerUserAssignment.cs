using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VelozientComputers.Core.Entities
{
    // <summary>
    /// Represents an assignment of a computer to a user.
    /// </summary>
    [Table("lnk_computer_user")]
    public class ComputerUserAssignment : BaseEntity
    {
        /// <summary>
        /// Gets or sets the unique identifier for the assignment.
        /// </summary>
        [Key]
        [Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the user ID for this assignment.
        /// </summary>
        [Required]
        [Column("user_id")]
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the computer ID for this assignment.
        /// </summary>
        [Required]
        [Column("computer_id")]
        public int ComputerId { get; set; }

        /// <summary>
        /// Gets or sets the date when the assignment started.
        /// </summary>
        [Required]
        [Column("assign_start_dt")]
        public DateTime AssignStartDate { get; set; }

        /// <summary>
        /// Gets or sets the date when the assignment ended.
        /// Null indicates an ongoing assignment.
        /// </summary>
        [Column("assign_end_dt")]
        public DateTime? AssignEndDate { get; set; }

        /// <summary>
        /// Gets or sets the user associated with this assignment.
        /// </summary>
        [ForeignKey("UserId")]
        public User User { get; set; }

        /// <summary>
        /// Gets or sets the computer associated with this assignment.
        /// </summary>
        [ForeignKey("ComputerId")]
        public Computer Computer { get; set; }
    }
}
