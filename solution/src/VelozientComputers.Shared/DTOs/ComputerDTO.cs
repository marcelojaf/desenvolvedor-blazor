using VelozientComputers.Core.Enums;
using VelozientComputers.Core.Extensions;

namespace VelozientComputers.Shared.DTOs
{
    /// <summary>
    /// Data transfer object for computer operations
    /// </summary>
    public class ComputerDTO
    {
        /// <summary>
        /// Gets or sets the computer identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the manufacturer name
        /// </summary>
        public string Manufacturer { get; set; }

        /// <summary>
        /// Gets or sets the computer serial number
        /// </summary>
        public string SerialNumber { get; set; }

        /// <summary>
        /// Gets or sets the computer current status
        /// </summary>
        public ComputerStatusEnum Status { get; set; }

        /// <summary>
        /// Gets the formatted display name for the status
        /// </summary>
        public string StatusDisplayName => Status.ToDisplayName();

        /// <summary>
        /// Gets or sets the computer purchase date
        /// </summary>
        public DateTime PurchaseDate { get; set; }

        /// <summary>
        /// Gets or sets the computer warranty expiration date
        /// </summary>
        public DateTime WarrantyExpirationDate { get; set; }

        /// <summary>
        /// Gets or sets the computer specifications
        /// </summary>
        public string Specifications { get; set; }

        /// <summary>
        /// Gets or sets the computer image URL
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the current user assignment
        /// </summary>
        public UserAssignmentDTO CurrentAssignment { get; set; }
    }
}