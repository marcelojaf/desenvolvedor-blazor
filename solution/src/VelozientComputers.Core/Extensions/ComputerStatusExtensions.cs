using VelozientComputers.Core.Enums;

namespace VelozientComputers.Core.Extensions
{
    /// <summary>
    /// Extension methods for ComputerStatus enum
    /// </summary>
    public static class ComputerStatusExtensions
    {
        /// <summary>
        /// Converts a status string to its enum value
        /// </summary>
        /// <param name="status">The status string from database</param>
        /// <returns>The corresponding ComputerStatus enum value</returns>
        public static ComputerStatusEnum ToComputerStatusEnum(this string status)
        {
            return status?.ToLower() switch
            {
                "new" => ComputerStatusEnum.New,
                "in_use" => ComputerStatusEnum.InUse,
                "available" => ComputerStatusEnum.Available,
                "in_maintenance" => ComputerStatusEnum.InMaintenance,
                "retired" => ComputerStatusEnum.Retired,
                _ => throw new ArgumentException($"Invalid status: {status}")
            };
        }

        /// <summary>
        /// Gets a user-friendly display name for the status
        /// </summary>
        /// <param name="status">The ComputerStatus enum value</param>
        /// <returns>A formatted string for display</returns>
        public static string ToDisplayName(this ComputerStatusEnum status)
        {
            return status switch
            {
                ComputerStatusEnum.New => "New",
                ComputerStatusEnum.InUse => "In Use",
                ComputerStatusEnum.Available => "Available",
                ComputerStatusEnum.InMaintenance => "In Maintenance",
                ComputerStatusEnum.Retired => "Retired",
                _ => throw new ArgumentException($"Invalid status: {status}")
            };
        }
    }
}