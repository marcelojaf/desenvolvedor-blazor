using System.ComponentModel.DataAnnotations;

namespace VelozientComputers.Core.Entities
{
    /// <summary>
    /// Represents the assignment of a computer to a user in the inventory management system.
    /// </summary>
    public class ComputerAssignment : BaseEntity
    {
        /// <summary>
        /// Gets or sets the unique identifier for the computer assignment.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the computer being assigned.
        /// </summary>
        [Required]
        public int ComputerId { get; set; }

        /// <summary>
        /// Gets or sets the computer object associated with this assignment.
        /// Represents the navigation property to the related Computer entity.
        /// </summary>
        public Computer Computer { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user receiving the computer.
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the user object associated with this assignment.
        /// Represents the navigation property to the related User entity.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Gets or sets the date when the computer was assigned to the user.
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        public DateTime AssignmentStartDate { get; set; }

        /// <summary>
        /// Gets or sets the date when the computer assignment ended.
        /// Null value indicates an ongoing assignment.
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime? AssignmentEndDate { get; set; }
    }
}