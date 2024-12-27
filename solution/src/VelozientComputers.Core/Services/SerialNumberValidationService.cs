using VelozientComputers.Core.Interfaces.Repository;
using VelozientComputers.Core.Interfaces.Service;
using VelozientComputers.Core.Validation;

namespace VelozientComputers.Core.Services;

/// <summary>
/// Service implementation for serial number validation.
/// </summary>
public class SerialNumberValidationService : ISerialNumberValidationService
{
    private readonly IManufacturerRepository _manufacturerRepository;
    private readonly IComputerRepository _computerRepository;

    /// <summary>
    /// Initializes a new instance of the SerialNumberValidationService.
    /// </summary>
    /// <param name="manufacturerRepository">The manufacturer repository.</param>
    /// <param name="computerRepository">The computer repository.</param>
    public SerialNumberValidationService(IManufacturerRepository manufacturerRepository, IComputerRepository computerRepository)
    {
        _manufacturerRepository = manufacturerRepository;
        _computerRepository = computerRepository;
    }

    /// <inheritdoc/>
    public async Task<bool> IsSerialNumberUniqueAsync(string serialNumber, int? excludeComputerId = null)
    {
        return !await _computerRepository.ExistsAsync(c =>
            c.SerialNumber == serialNumber &&
            (!excludeComputerId.HasValue || c.Id != excludeComputerId.Value));
    }

    /// <inheritdoc/>
    public async Task<(bool IsValid, string ErrorMessage)> ValidateSerialNumberAsync(string serialNumber, int manufacturerId)
    {
        if (string.IsNullOrEmpty(serialNumber))
            return (false, "Serial number is required.");

        // Verify manufacturer exists in database
        var manufacturer = await _manufacturerRepository.GetByIdAsync(manufacturerId);
        if (manufacturer == null)
            return (false, "Invalid manufacturer.");

        // Use core validation logic
        return SerialNumberValidation.ValidateSerialNumber(serialNumber, manufacturerId);
    }
}