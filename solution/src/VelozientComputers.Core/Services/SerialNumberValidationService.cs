using VelozientComputers.Core.Interfaces.Repository;
using VelozientComputers.Core.Interfaces.Service;

namespace VelozientComputers.Core.Services
{
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
        public async Task<(bool IsValid, string ErrorMessage)> ValidateSerialNumberAsync(string? serialNumber, int manufacturerId)
        {
            if (string.IsNullOrEmpty(serialNumber))
                return (false, "Serial number is required.");

            var manufacturer = await _manufacturerRepository.GetByIdAsync(manufacturerId);

            if (manufacturer == null)
                return (false, "Invalid manufacturer.");

            if (string.IsNullOrEmpty(manufacturer.SerialRegex))
                return (false, $"No serial number pattern defined for manufacturer {manufacturer.Name}.");

            try
            {
                var regex = new System.Text.RegularExpressions.Regex(manufacturer.SerialRegex,
                    System.Text.RegularExpressions.RegexOptions.Compiled);

                var isValid = regex.IsMatch(serialNumber);
                return isValid
                    ? (true, string.Empty)
                    : (false, $"Invalid serial number format for {manufacturer.Name}.");
            }
            catch (System.ArgumentException)
            {
                return (false, $"Invalid regex pattern for manufacturer {manufacturer.Name}.");
            }
        }
    }
}