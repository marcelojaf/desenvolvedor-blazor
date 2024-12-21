using System.ComponentModel.DataAnnotations;
using VelozientComputers.Core.Entities;

namespace VelozientComputers.Core.Validations
{
    /// <summary>
    /// Validation attribute for computer serial numbers that validates against manufacturer-specific patterns.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class SerialNumberAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // Get the validation service from the service provider
            var validationService = (ISerialNumberValidationService?)validationContext.GetService(typeof(ISerialNumberValidationService));

            if (validationService == null)
                return new ValidationResult("Serial number validation service not available.");

            if (validationContext.ObjectInstance is not Computer computer)
                return new ValidationResult("SerialNumberAttribute can only be used with Computer objects.");

            // Execute the validation asynchronously
            var validationTask = validationService.ValidateSerialNumberAsync(
                value?.ToString(),
                computer.ComputerManufacturerId);

            // Wait for the result - in a validation attribute we have to block
            var (isValid, errorMessage) = validationTask.ConfigureAwait(false).GetAwaiter().GetResult();

            return isValid ? ValidationResult.Success : new ValidationResult(errorMessage);
        }
    }
}