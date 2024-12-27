namespace VelozientComputers.Core.Interfaces.Service
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
        Task<(bool IsValid, string ErrorMessage)> ValidateSerialNumberAsync(string serialNumber, int manufacturerId);

        /// <summary>
        /// Checks if a serial number is unique in the system.
        /// </summary>
        /// <param name="serialNumber">The serial number to check for uniqueness.</param>
        /// <param name="excludeComputerId">Optional. The ID of a computer to exclude from the uniqueness check, useful when updating an existing computer.</param>
        /// <returns>True if the serial number is unique, false if it already exists.</returns>
        Task<bool> IsSerialNumberUniqueAsync(string serialNumber, int? excludeComputerId = null);
    }
}
