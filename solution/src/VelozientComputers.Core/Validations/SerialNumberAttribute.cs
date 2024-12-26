using System.ComponentModel.DataAnnotations;
using VelozientComputers.Core.Entities;
using VelozientComputers.Core.Interfaces.Service;
using VelozientComputers.Core.Validation;

namespace VelozientComputers.Core.Validations
{
    /// <summary>
    /// Validation attribute for computer serial numbers that validates against manufacturer-specific patterns.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class SerialNumberAttribute : ValidationAttribute
    {
        /// <summary>
        /// Validates if the provided serial number matches both basic format rules and
        /// manufacturer-specific pattern requirements.
        /// </summary>
        /// <param name="value">The serial number value to validate.</param>
        /// <param name="validationContext">The validation context which provides additional data.</param>
        /// <returns>ValidationResult.Success if validation passes, ValidationResult with error message otherwise.</returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("Serial number is required.");
            }

            // Get manufacturer property value through reflection
            var manufacturerProperty = validationContext.ObjectType.GetProperty("Manufacturer");
            if (manufacturerProperty == null)
            {
                return new ValidationResult("Invalid model: Manufacturer property not found.");
            }

            var manufacturer = manufacturerProperty.GetValue(validationContext.ObjectInstance)?.ToString();
            if (string.IsNullOrEmpty(manufacturer))
            {
                return ValidationResult.Success;
            }

            // Convert manufacturer name to ID
            int manufacturerId = manufacturer.ToLower() switch
            {
                "apple" => 1,
                "dell" => 2,
                "hp" => 3,
                "lenovo" => 4,
                _ => 0
            };

            if (manufacturerId == 0)
            {
                return new ValidationResult("Invalid manufacturer.");
            }

            var serialNumber = value.ToString();

            // Validate using the core validation logic
            var (isValid, errorMessage) = SerialNumberValidation.ValidateSerialNumber(
                serialNumber!,
                manufacturerId);

            return isValid
                ? ValidationResult.Success
                : new ValidationResult(errorMessage);
        }
    }
}