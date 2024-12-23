using VelozientComputers.Core.Entities;

namespace VelozientComputers.Core.Interfaces.Service
{ 
    /// <summary>
    /// Service interface for managing computer assignments
    /// </summary>
    public interface IAssignmentService
    {
        #region Query Methods

        /// <summary>
        /// Gets the current assignment for a computer
        /// </summary>
        /// <param name="computerId">Computer identifier</param>
        /// <returns>Current assignment details if exists, null otherwise</returns>
        Task<ComputerAssignment> GetCurrentAssignmentAsync(int computerId);

        /// <summary>
        /// Gets all assignments for a computer
        /// </summary>
        /// <param name="computerId">Computer identifier</param>
        /// <returns>Collection of all assignments for the computer</returns>
        Task<IEnumerable<ComputerAssignment>> GetComputerAssignmentHistoryAsync(int computerId);

        /// <summary>
        /// Gets all assignments for a user
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>Collection of all assignments for the user</returns>
        Task<IEnumerable<ComputerAssignment>> GetUserAssignmentHistoryAsync(int userId);

        #endregion

        #region Command Methods

        /// <summary>
        /// Assigns a computer to a user
        /// </summary>
        /// <param name="computerAssignment">Assignment details</param>
        /// <returns>Created assignment details</returns>
        Task<ComputerAssignment> AssignComputerAsync(ComputerAssignment computerAssignment);

        /// <summary>
        /// Ends a computer assignment
        /// </summary>
        /// <param name="computerId">Computer identifier</param>
        /// <param name="endDate">End date of the assignment</param>
        /// <returns>Updated assignment details</returns>
        Task<ComputerAssignment> EndAssignmentAsync(int computerId, DateTime endDate);

        #endregion
    }
}