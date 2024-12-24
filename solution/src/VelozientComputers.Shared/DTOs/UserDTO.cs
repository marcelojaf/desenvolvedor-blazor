namespace VelozientComputers.Shared.DTOs
{
    /// <summary>
    /// Data transfer object for user operations
    /// </summary>
    public class UserDTO
    {
        /// <summary>
        /// Gets or sets the user identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the user's first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the user's last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the user's email address
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the list of computer assignments for this user
        /// </summary>
        public IEnumerable<ComputerAssignmentDTO> ComputerAssignments { get; set; }
    }
}