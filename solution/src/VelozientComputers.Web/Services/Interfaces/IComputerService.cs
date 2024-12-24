using VelozientComputers.Shared.DTOs;

namespace VelozientComputers.Web.Services.Interfaces;

/// <summary>
/// Interface for computer-related HTTP operations
/// </summary>
public interface IComputerService : IBaseService<ComputerDTO, CreateComputerDTO, UpdateComputerDTO>
{
    /// <summary>
    /// Gets computers with expiring warranty
    /// </summary>
    /// <param name="daysThreshold">Days threshold for warranty expiration</param>
    /// <returns>Collection of computers with warranty expiring soon</returns>
    Task<IEnumerable<ComputerDTO>> GetComputersWithExpiringWarrantyAsync(int daysThreshold = 30);

    /// <summary>
    /// Validates a computer's serial number
    /// </summary>
    /// <param name="manufacturer">Manufacturer name</param>
    /// <param name="serialNumber">Serial number to validate</param>
    /// <returns>True if valid, false otherwise</returns>
    Task<bool> ValidateSerialNumberAsync(string manufacturer, string serialNumber);
}