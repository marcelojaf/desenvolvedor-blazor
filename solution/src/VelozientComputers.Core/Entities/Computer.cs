using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VelozientComputers.Core.Entities
{
    /// <summary>
    /// Represents a computer in the inventory system.
    /// </summary>
    [Table("computer")]
    public class Computer : BaseEntity
    {
        /// <summary>
        /// Gets or sets the unique identifier for the computer.
        /// </summary>
        [Key]
        [Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the manufacturer ID of the computer.
        /// </summary>
        [Required]
        [Column("computer_manufacturer_id")]
        public int ComputerManufacturerId { get; set; }

        /// <summary>
        /// Gets or sets the manufacturer of the computer.
        /// </summary>
        [ForeignKey("ComputerManufacturerId")]
        public ComputerManufacturer Manufacturer { get; set; }

        /// <summary>
        /// Gets or sets the serial number of the computer.
        /// </summary>
        [Required]
        [Column("serial_number")]
        public string SerialNumber { get; set; }

        /// <summary>
        /// Gets or sets the specifications of the computer.
        /// </summary>
        [Column("specifications")]
        public string Specifications { get; set; }

        /// <summary>
        /// Gets or sets the URL of the computer's image.
        /// </summary>
        [Column("image_url")]
        public string ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the purchase date of the computer.
        /// </summary>
        [Column("purchase_dt")]
        public DateTime PurchaseDate { get; set; }

        /// <summary>
        /// Gets or sets the warranty expiration date of the computer.
        /// </summary>
        [Column("warranty_expiration_dt")]
        public DateTime WarrantyExpirationDate { get; set; }

        /// <summary>
        /// Gets or sets the creation date of the computer record.
        /// </summary>
        [Column("create_dt")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Gets or sets the collection of status assignments for this computer.
        /// </summary>
        public ICollection<ComputerStatusAssignment> StatusAssignments { get; set; }

        /// <summary>
        /// Gets or sets the collection of user assignments for this computer.
        /// </summary>
        public ICollection<ComputerUserAssignment> UserAssignments { get; set; }
    }
}