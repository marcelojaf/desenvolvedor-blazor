using System.ComponentModel.DataAnnotations;
using VelozientComputers.Infrastructure.Data;

namespace VelozientComputers.Shared.DTOs
{
    /// <summary>
    /// Data transfer object for user update operations
    /// </summary>
    public class UpdateUserDTO
    {
        /// <summary>
        /// Gets or sets the user's first name
        /// </summary>
        [Required(ErrorMessage = "First name is required")]
        [StringLength(DbConstants.NameLength, ErrorMessage = "First name cannot exceed {1} characters")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the user's last name
        /// </summary>
        [Required(ErrorMessage = "Last name is required")]
        [StringLength(DbConstants.NameLength, ErrorMessage = "Last name cannot exceed {1} characters")]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the user's email address
        /// </summary>
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [StringLength(DbConstants.EmailLength, ErrorMessage = "Email cannot exceed {1} characters")]
        public string Email { get; set; }
    }
}