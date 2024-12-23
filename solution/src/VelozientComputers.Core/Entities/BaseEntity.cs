using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VelozientComputers.Core.Entities
{
    /// <summary>
    /// Base class for all entities in the system.
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Gets or sets the unique identifier for the entity.
        /// </summary>
        [Key]
        [Column("id")]
        public int Id { get; set; }


    }
}