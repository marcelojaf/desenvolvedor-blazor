using System.ComponentModel.DataAnnotations;

namespace VelozientComputers.Api.DTOs
{
    /// <summary>
    /// Data transfer object for computer update operations
    /// </summary>
    public class UpdateComputerDTO
    {
        /// <summary>
        /// Gets or sets the computer status
        /// </summary>
        [Required(ErrorMessage = "Status is required")]
        public string Status { get; set; }

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