using System.ComponentModel.DataAnnotations;
using VelozientComputers.Core.Validations;

namespace VelozientComputers.Shared.DTOs
{
    /// <summary>
    /// Data transfer object for computer update operations
    /// </summary>
    public class UpdateComputerDTO
    {
        /// <summary>
        /// Gets or sets the manufacturer name
        /// </summary>
        [Required(ErrorMessage = "Manufacturer is required")]
        public string Manufacturer { get; set; }

        /// <summary>
        /// Gets or sets the computer serial number
        /// </summary>
        [Required(ErrorMessage = "Serial number is required")]
        [SerialNumber]
        public string SerialNumber { get; set; }

        /// <summary>
        /// Gets or sets the computer status
        /// </summary>
        [Required(ErrorMessage = "Status is required")]
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the computer purchase date
        /// </summary>
        [Required(ErrorMessage = "Purchase date is required")]
        public DateTime PurchaseDate { get; set; }

        /// <summary>
        /// Gets or sets the computer warranty expiry date
        /// </summary>
        [Required(ErrorMessage = "Warranty expiry date is required")]
        public DateTime WarrantyExpiryDate { get; set; }

        /// <summary>
        /// Gets or sets the computer specifications
        /// </summary>
        [Required(ErrorMessage = "Specifications are required")]
        public string Specifications { get; set; }

        /// <summary>
        /// Gets or sets the computer image URL
        /// </summary>
        public string ImageUrl { get; set; }
    }
}