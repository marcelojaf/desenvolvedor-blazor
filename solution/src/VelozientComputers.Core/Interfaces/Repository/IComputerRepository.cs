using VelozientComputers.Core.Entities;

namespace VelozientComputers.Core.Interfaces.Repository
{
    /// <summary>
    /// Specific repository interface for Computer entity
    /// </summary>
    public interface IComputerRepository : IRepository<Computer>
    {
        #region Query Methods

        /// <summary>
        /// Gets a computer with its current assignments
        /// </summary>
        /// <param name="id">Computer identifier</param>
        /// <returns>Computer with assignment data or null if not found</returns>
        Task<Computer> GetWithCurrentAssignmentAsync(int id);

        /// <summary>
        /// Gets all computers with their current assignments
        /// </summary>
        /// <returns>Collection of computers with assignment data</returns>
        Task<IEnumerable<Computer>> GetAllWithCurrentAssignmentsAsync();

        #endregion
    }
}