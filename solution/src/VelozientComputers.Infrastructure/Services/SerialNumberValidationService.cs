using Microsoft.EntityFrameworkCore;
using VelozientComputers.Core.Validations;
using VelozientComputers.Infrastructure.Data;

namespace VelozientComputers.Infrastructure.Services
{
    /// <summary>
    /// Service implementation for serial number validation.
    /// </summary>
    public class SerialNumberValidationService : ISerialNumberValidationService
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the SerialNumberValidationService.
        /// </summary>
        /// <param name="context">The database context.</param>
        public SerialNumberValidationService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<(bool IsValid, string? ErrorMessage)> ValidateSerialNumberAsync(string? serialNumber, int manufacturerId)
        {
            if (string.IsNullOrEmpty(serialNumber))
                return (false, "Serial number is required.");

            var manufacturer = await _context.ComputerManufacturers
                .FirstOrDefaultAsync(m => m.Id == manufacturerId);

            if (manufacturer == null)
                return (false, "Invalid manufacturer.");

            if (string.IsNullOrEmpty(manufacturer.SerialRegex))
                return (false, $"No serial number pattern defined for manufacturer {manufacturer.Name}.");

            var isValid = System.Text.RegularExpressions.Regex.IsMatch(serialNumber, manufacturer.SerialRegex);
            return isValid
                ? (true, null)
                : (false, $"Invalid serial number format for {manufacturer.Name}.");
        }
    }
}
