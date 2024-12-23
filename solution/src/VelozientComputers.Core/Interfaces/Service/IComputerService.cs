using VelozientComputers.Core.Entities;

namespace VelozientComputers.Core.Interfaces.Service
{
    /// <summary>
    /// Service interface for managing computer operations
    /// </summary>
    public interface IComputerService
    {
        #region Query Methods

        /// <summary>
        /// Gets all computers with their current assignments
        /// </summary>
        /// <returns>Collection of computers with assignment information</returns>
        Task<IEnumerable<Computer>> GetAllComputersAsync();

        /// <summary>
        /// Gets a specific computer by its identifier
        /// </summary>
        /// <param name="id">Computer identifier</param>
        /// <returns>Computer details if found, null otherwise</returns>
        Task<Computer> GetComputerByIdAsync(int id);

        /// <summary>
        /// Gets computers with warranty status approaching expiration
        /// </summary>
        /// <param name="daysThreshold">Days threshold for warranty expiration</param>
        /// <returns>Collection of computers with expiring warranty</returns>
        Task<IEnumerable<Computer>> GetComputersWithExpiringWarrantyAsync(int daysThreshold = 30);

        #endregion

        #region Command Methods

        /// <summary>
        /// Creates a new computer
        /// </summary>
        /// <param name="computer">Computer creation data</param>
        /// <returns>Created computer details</returns>
        Task<Computer> CreateComputerAsync(Computer computer);

        /// <summary>
        /// Updates an existing computer
        /// </summary>
        /// <param name="id">Computer identifier</param>
        /// <param name="computer">Computer update data</param>
        /// <returns>Updated computer details</returns>
        Task<Computer> UpdateComputerAsync(int id, Computer computer);

        /// <summary>
        /// Updates the status of a computer
        /// </summary>
        /// <param name="id">Computer identifier</param>
        /// <param name="status">Status update data</param>
        /// <returns>Updated computer details</returns>
        Task<Computer> UpdateComputerStatusAsync(int id, Computer status);

        /// <summary>
        /// Deletes a computer
        /// </summary>
        /// <param name="id">Computer identifier</param>
        Task DeleteComputerAsync(int id);

        /// <summary>
        /// Validates a computer's serial number based on manufacturer requirements
        /// </summary>
        /// <param name="manufacturer">Computer manufacturer</param>
        /// <param name="serialNumber">Serial number to validate</param>
        /// <returns>True if valid, false otherwise</returns>
        Task<bool> ValidateSerialNumberAsync(string manufacturer, string serialNumber);

        #endregion
    }
}