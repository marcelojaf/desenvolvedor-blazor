using VelozientComputers.Core.Entities;

namespace VelozientComputers.Core.Interfaces.Repository
{
    /// <summary>
    /// Repository interface for computer manufacturer operations
    /// </summary>
    public interface IManufacturerRepository : IRepository<ComputerManufacturer>
    {
        /// <summary>
        /// Gets a manufacturer by name
        /// </summary>
        /// <param name="name">Name of the manufacturer</param>
        /// <returns>Manufacturer if found, null otherwise</returns>
        Task<ComputerManufacturer> GetByNameAsync(string name);
    }
}