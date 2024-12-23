using System.ComponentModel.DataAnnotations;
using VelozientComputers.Core.Entities;
using VelozientComputers.Core.Interfaces.Service;

namespace VelozientComputers.Core.Validations
{
    /// <summary>
    /// Validation attribute for computer serial numbers that validates against manufacturer-specific patterns.
    /// This attribute must be used only on Computer entity properties.
    /// </summary>
    /// <remarks>
    /// This attribute performs two levels of validation:
    /// 1. Basic format validation (length, allowed characters)
    /// 2. Manufacturer-specific pattern validation using ISerialNumberValidationService
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class SerialNumberAttribute : ValidationAttribute
    {
        // Maximum allowed length for serial numbers
        private const int MaxSerialNumberLength = 50;

        // Pattern for allowed characters (uppercase letters, numbers, and hyphens)
        private const string AllowedCharactersPattern = @"^[A-Z0-9\-]+$";

        /// <summary>
        /// Validates if the provided serial number matches both basic format rules and
        /// manufacturer-specific pattern requirements.
        /// </summary>
        /// <param name="value">The serial number value to validate.</param>
        /// <param name="validationContext">The validation context which provides additional data.</param>
        /// <returns>ValidationResult.Success if validation passes, ValidationResult with error message otherwise.</returns>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // Validate null value
            if (value == null)
            {
                return new ValidationResult("Serial number is required.");
            }

            var serialNumber = value.ToString();

            // Validate serial number length
            if (serialNumber?.Length > MaxSerialNumberLength)
            {
                return new ValidationResult($"Serial number cannot exceed {MaxSerialNumberLength} characters.");
            }

            // Validate allowed characters
            if (!System.Text.RegularExpressions.Regex.IsMatch(serialNumber!, AllowedCharactersPattern))
            {
                return new ValidationResult("Serial number can only contain uppercase letters, numbers, and hyphens.");
            }

            // Get the validation service
            var validationService = (ISerialNumberValidationService?)validationContext.GetService(typeof(ISerialNumberValidationService))
                ?? throw new InvalidOperationException("Serial number validation service is not registered in the dependency injection container.");

            // Ensure the attribute is used on a Computer object
            if (validationContext.ObjectInstance is not Computer computer)
            {
                return new ValidationResult("SerialNumberAttribute can only be used with Computer entity.");
            }

            try
            {
                // Perform manufacturer-specific validation
                var validationTask = validationService.ValidateSerialNumberAsync(
                    serialNumber,
                    computer.ComputerManufacturerId);

                // Since validation attributes don't support async operations,
                // we need to block here to get the result
                var (isValid, errorMessage) = validationTask
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();

                return isValid
                    ? ValidationResult.Success
                    : new ValidationResult(errorMessage);
            }
            catch (Exception ex)
            {
                // Log the exception in a production environment
                return new ValidationResult($"An error occurred during serial number validation: {ex.Message}");
            }
        }
    }
}