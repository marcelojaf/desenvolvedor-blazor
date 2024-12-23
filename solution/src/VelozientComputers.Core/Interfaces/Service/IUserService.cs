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

        /// <summary>
        /// Gets a user by their email address
        /// </summary>
        /// <param name="email">Email address</param>
        /// <returns>User details if found, null otherwise</returns>
        Task<User> GetUserByEmailAsync(string email);

        #endregion

        #region Command Methods

        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="user">User creation data</param>
        /// <returns>Created user details</returns>
        Task<User> CreateUserAsync(User user);

        /// <summary>
        /// Updates an existing user
        /// </summary>
        /// <param name="id">User identifier</param>
        /// <param name="user">User update data</param>
        /// <returns>Updated user details</returns>
        Task<User> UpdateUserAsync(int id, User user);

        /// <summary>
        /// Deletes a user
        /// </summary>
        /// <param name="id">User identifier</param>
        Task DeleteUserAsync(int id);

        #endregion
    }
}