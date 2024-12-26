using System.Text.RegularExpressions;

namespace VelozientComputers.Core.Validation;

/// <summary>
/// Contains core validation logic for serial numbers
/// </summary>
public static class SerialNumberValidation
{
    /// <summary>
    /// Dictionary mapping manufacturer IDs to their respective validation patterns
    /// </summary>
    public static readonly IReadOnlyDictionary<int, (string Manufacturer, string Pattern)> ManufacturerPatterns =
        new Dictionary<int, (string Manufacturer, string Pattern)>
    {
        { 1, ("Apple", @"^[A-Z]{3}[C-Z0-9][1-9C-NP-RTY][A-Z0-9]{3}[A-Z0-9]{4}$") },
        { 2, ("Dell", @"^[A-Z0-9]{7}$") },
        { 3, ("HP", @"^[A-Z0-9]{3}\d{3}[A-Z0-9]{4}$") },
        { 4, ("Lenovo", @"^\d{2}-[A-Z0-9]{5}$") }
    };

    /// <summary>
    /// Validates a serial number against the pattern for a specific manufacturer
    /// </summary>
    /// <param name="serialNumber">The serial number to validate</param>
    /// <param name="manufacturerId">The manufacturer ID</param>
    /// <returns>A tuple containing the validation result and any error message</returns>
    public static (bool IsValid, string ErrorMessage) ValidateSerialNumber(string serialNumber, int manufacturerId)
    {
        if (!ManufacturerPatterns.TryGetValue(manufacturerId, out var manufacturerInfo))
        {
            return (false, $"Invalid manufacturer ID: {manufacturerId}");
        }

        if (!Regex.IsMatch(serialNumber, manufacturerInfo.Pattern))
        {
            return (false, $"Invalid serial number format for {manufacturerInfo.Manufacturer}");
        }

        return (true, null);
    }
}