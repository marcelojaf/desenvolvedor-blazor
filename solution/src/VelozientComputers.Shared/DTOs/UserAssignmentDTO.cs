namespace VelozientComputers.Shared.DTOs
{
    /// <summary>
    /// Data transfer object for user assignment operations
    /// </summary>
    public class UserAssignmentDTO
    {
        /// <summary>
        /// Gets or sets the user identifier
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the user's full name
        /// </summary>
        public string UserFullName { get; set; }

        /// <summary>
        /// Gets or sets the user's email address
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the assignment start date
        /// </summary>
        public DateTime AssignStartDate { get; set; }

        /// <summary>
        /// Gets or sets the assignment end date
        /// </summary>
        public DateTime? AssignEndDate { get; set; }
    }
}