using VelozientComputers.Core.Entities;

namespace VelozientComputers.Core.Interfaces.Repository
{
    /// <summary>
    /// Repository interface for computer status operations
    /// </summary>
    public interface IStatusRepository : IRepository<ComputerStatus>
    {
        /// <summary>
        /// Gets a status by its localized name
        /// </summary>
        /// <param name="name">Localized name of the status</param>
        /// <returns>Status if found, null otherwise</returns>
        Task<ComputerStatus> GetByNameAsync(string name);
    }
}