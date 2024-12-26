namespace VelozientComputers.Core.Enums
{
    /// <summary>
    /// Represents the possible states of a computer in the system
    /// </summary>
    public enum ComputerStatusEnum
    {
        /// <summary>
        /// The laptop is brand new and has not been assigned or used
        /// </summary>
        New = 1,

        /// <summary>
        /// The laptop is currently assigned to and being used by someone
        /// </summary>
        InUse = 2,

        /// <summary>
        /// The laptop is ready for use but not currently assigned to anyone
        /// </summary>
        Available = 3,

        /// <summary>
        /// The laptop is undergoing repairs or maintenance
        /// </summary>
        InMaintenance = 4,

        /// <summary>
        /// The laptop has been decommissioned and is no longer in service
        /// </summary>
        Retired = 5
    }
}