using VelozientComputers.Core.Entities;

namespace VelozientComputers.Infrastructure.Repository
{
    /// <summary>
    /// Specific repository interface for User entity
    /// </summary>
    public interface IUserRepository : IRepository<User>
    {
        #region Query Methods

        /// <summary>
        /// Gets a user with their currently assigned computers
        /// </summary>
        /// <param name="id">User identifier</param>
        /// <returns>User with computer data or null if not found</returns>
        Task<User> GetWithCurrentComputersAsync(int id);

        /// <summary>
        /// Gets all users with their currently assigned computers
        /// </summary>
        /// <returns>Collection of users with computer data</returns>
        Task<IEnumerable<User>> GetAllWithCurrentComputersAsync();

        #endregion
    }
}