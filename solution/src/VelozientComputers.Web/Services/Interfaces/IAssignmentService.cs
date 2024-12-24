using VelozientComputers.Shared.DTOs;

namespace VelozientComputers.Web.Services.Interfaces;

/// <summary>
/// Interface for computer assignment operations
/// </summary>
public interface IAssignmentService
{
    /// <summary>
    /// Gets the current assignment for a computer
    /// </summary>
    /// <param name="computerId">Computer identifier</param>
    /// <returns>Current user assignment if exists</returns>
    Task<UserAssignmentDTO> GetCurrentAssignmentAsync(int computerId);

    /// <summary>
    /// Gets the assignment history for a computer
    /// </summary>
    /// <param name="computerId">Computer identifier</param>
    /// <returns>Collection of user assignments</returns>
    Task<IEnumerable<UserAssignmentDTO>> GetComputerAssignmentHistoryAsync(int computerId);

    /// <summary>
    /// Gets the assignment history for a user
    /// </summary>
    /// <param name="userId">User identifier</param>
    /// <returns>Collection of computer assignments</returns>
    Task<IEnumerable<ComputerAssignmentDTO>> GetUserAssignmentHistoryAsync(int userId);

    /// <summary>
    /// Assigns a computer to a user
    /// </summary>
    /// <param name="computerId">Computer identifier</param>
    /// <param name="userId">User identifier</param>
    /// <returns>New assignment details</returns>
    Task<UserAssignmentDTO> AssignComputerToUserAsync(int computerId, int userId);

    /// <summary>
    /// Ends the current assignment for a computer
    /// </summary>
    /// <param name="computerId">Computer identifier</param>
    /// <returns>True if unassignment was successful</returns>
    Task<UserAssignmentDTO> EndAssignmentAsync(int computerId);
}