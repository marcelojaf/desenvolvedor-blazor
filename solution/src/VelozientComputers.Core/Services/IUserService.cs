using VelozientComputers.Api.DTOs;

namespace VelozientComputers.Core.Services
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
        Task<IEnumerable<UserListDto>> GetAllUsersAsync();

        /// <summary>
        /// Gets a specific user by their identifier
        /// </summary>
        /// <param name="id">User identifier</param>
        /// <returns>User details if found, null otherwise</returns>
        Task<UserListDto> GetUserByIdAsync(int id);

        /// <summary>
        /// Gets a user by their email address
        /// </summary>
        /// <param name="email">Email address</param>
        /// <returns>User details if found, null otherwise</returns>
        Task<UserListDto> GetUserByEmailAsync(string email);

        #endregion

        #region Command Methods

        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="userDto">User creation data</param>
        /// <returns>Created user details</returns>
        Task<UserListDto> CreateUserAsync(CreateUserDto userDto);

        /// <summary>
        /// Updates an existing user
        /// </summary>
        /// <param name="id">User identifier</param>
        /// <param name="userDto">User update data</param>
        /// <returns>Updated user details</returns>
        Task<UserListDto> UpdateUserAsync(int id, UpdateUserDto userDto);

        /// <summary>
        /// Deletes a user
        /// </summary>
        /// <param name="id">User identifier</param>
        Task DeleteUserAsync(int id);

        #endregion
    }
}