using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VelozientComputers.Core.Entities
{
    /// <summary>
    /// Represents a status that a computer can have in the system.
    /// </summary>
    [Table("computer_status")]
    public class ComputerStatus : BaseEntity
    {
        /// <summary>
        /// Gets or sets the localized name of the status.
        /// </summary>
        [Required]
        [Column("localized_name")]
        public string LocalizedName { get; set; }

        /// <summary>
        /// Gets or sets the creation date of the entity.
        /// </summary>
        [Column("create_dt")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Gets or sets the collection of status assignments.
        /// </summary>
        public virtual ICollection<ComputerStatusAssignment> StatusAssignments { get; set; }
    }
}