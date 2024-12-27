using VelozientComputers.Core.Interfaces.Service;
using VelozientComputers.Core.Validation;

namespace VelozientComputers.Web.Services;

/// <summary>
/// Client-side implementation of serial number validation service
/// </summary>
public class ClientSerialNumberValidationService : ISerialNumberValidationService
{
    /// <summary>
    /// Validates a serial number against manufacturer-specific patterns
    /// </summary>
    /// <param name="serialNumber">The serial number to validate</param>
    /// <param name="manufacturerId">The manufacturer ID</param>
    /// <returns>Tuple containing validation result and error message if any</returns>
    public Task<(bool IsValid, string ErrorMessage)> ValidateSerialNumberAsync(string serialNumber, int manufacturerId)
    {
        var result = SerialNumberValidation.ValidateSerialNumber(serialNumber, manufacturerId);
        return Task.FromResult(result);
    }

    /// <inheritdoc/>
    public async Task<bool> IsSerialNumberUniqueAsync(string serialNumber, int? excludeComputerId = null)
    {
        throw new NotImplementedException("Not implemented on Web project");
    }
}