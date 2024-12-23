using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VelozientComputers.Core.Entities
{
    /// <summary>
    /// Represents a user in the system who can be assigned computers.
    /// </summary>
    [Table("user")]
    public class User : BaseEntity
    {
        /// <summary>
        /// Gets or sets the user's first name.
        /// </summary>
        [Required]
        [Column("first_name")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the user's last name.
        /// </summary>
        [Required]
        [Column("last_name")]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the user's email address.
        /// </summary>
        [Required]
        [EmailAddress]
        [Column("email_address")]
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the collection of computer assignments for this user.
        /// </summary>
        public virtual ICollection<ComputerUserAssignment> ComputerAssignments { get; set; }
    }
}