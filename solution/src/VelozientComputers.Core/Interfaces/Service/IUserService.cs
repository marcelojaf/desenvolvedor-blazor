using VelozientComputers.Core.Entities;

namespace VelozientComputers.Core.Interfaces.Service
{
    /// <summary>
    /// Service interface for managing user operations
    /// </summary>
    public interface IUserService
    {
        #region Query Methods

        /// <summary>
        /// Gets all users with their current computer assignments
        /// </summary>
        /// <returns>Collection of users with assignment information</returns>
        Task<IEnumerable<User>> GetAllUsersAsync();

        /// <summary>
        /// Gets a specific user by their identifier
        /// </summary>
        /// <param name="id">User identifier</param>
        /// <returns>User details if found, null otherwise</returns>
        Task<User> GetUserByIdAsync(int id);

        #endregion
    }
}