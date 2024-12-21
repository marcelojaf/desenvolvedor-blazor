namespace VelozientComputers.Core.Validations
{
    /// <summary>
    /// Interface for serial number validation service.
    /// </summary>
    public interface ISerialNumberValidationService
    {
        /// <summary>
        /// Validates a serial number for a specific manufacturer.
        /// </summary>
        /// <param name="serialNumber">The serial number to validate.</param>
        /// <param name="manufacturerId">The ID of the manufacturer.</param>
        /// <returns>True if the serial number is valid, false otherwise.</returns>
        Task<(bool IsValid, string? ErrorMessage)> ValidateSerialNumberAsync(string? serialNumber, int manufacturerId);
    }
}
