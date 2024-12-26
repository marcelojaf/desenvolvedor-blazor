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

    /// <summary>
    /// Initializes a new instance of the SerialNumberValidationService.
    /// </summary>
    /// <param name="manufacturerRepository">The manufacturer repository.</param>
    public SerialNumberValidationService(IManufacturerRepository manufacturerRepository)
    {
        _manufacturerRepository = manufacturerRepository;
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