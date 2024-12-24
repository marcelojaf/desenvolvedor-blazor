namespace VelozientComputers.Shared.DTOs
{
    /// <summary>
    /// Data transfer object for computer assignment operations
    /// </summary>
    public class ComputerAssignmentDTO
    {
        /// <summary>
        /// Gets or sets the computer identifier
        /// </summary>
        public int ComputerId { get; set; }

        /// <summary>
        /// Gets or sets the manufacturer name
        /// </summary>
        public string Manufacturer { get; set; }

        /// <summary>
        /// Gets or sets the computer serial number
        /// </summary>
        public string SerialNumber { get; set; }

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