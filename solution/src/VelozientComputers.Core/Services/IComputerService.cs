using VelozientComputers.Core.Entities;
using VelozientComputers.Api.DTOs;

namespace VelozientComputers.Core.Services
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
        Task<IEnumerable<ComputerListDto>> GetAllComputersAsync();

        /// <summary>
        /// Gets a specific computer by its identifier
        /// </summary>
        /// <param name="id">Computer identifier</param>
        /// <returns>Computer details if found, null otherwise</returns>
        Task<ComputerListDto> GetComputerByIdAsync(int id);

        /// <summary>
        /// Gets computers with warranty status approaching expiration
        /// </summary>
        /// <param name="daysThreshold">Days threshold for warranty expiration</param>
        /// <returns>Collection of computers with expiring warranty</returns>
        Task<IEnumerable<ComputerListDto>> GetComputersWithExpiringWarrantyAsync(int daysThreshold = 30);

        #endregion

        #region Command Methods

        /// <summary>
        /// Creates a new computer
        /// </summary>
        /// <param name="computerDto">Computer creation data</param>
        /// <returns>Created computer details</returns>
        Task<ComputerListDto> CreateComputerAsync(CreateComputerDto computerDto);

        /// <summary>
        /// Updates an existing computer
        /// </summary>
        /// <param name="id">Computer identifier</param>
        /// <param name="computerDto">Computer update data</param>
        /// <returns>Updated computer details</returns>
        Task<ComputerListDto> UpdateComputerAsync(int id, UpdateComputerDto computerDto);

        /// <summary>
        /// Updates the status of a computer
        /// </summary>
        /// <param name="id">Computer identifier</param>
        /// <param name="statusDto">Status update data</param>
        /// <returns>Updated computer details</returns>
        Task<ComputerListDto> UpdateComputerStatusAsync(int id, UpdateComputerStatusDto statusDto);

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