using VelozientComputers.Core.Entities;
using VelozientComputers.Core.Interfaces.Repository;
using VelozientComputers.Core.Interfaces.Service;

namespace VelozientComputers.Core.Services
{
    /// <summary>
    /// Service implementation for managing user operations
    /// </summary>
    public class UserService : IUserService
    {
        #region Fields

        private readonly IUserRepository _userRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the UserService class
        /// </summary>
        /// <param name="userRepository">User repository instance</param>
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        #endregion

        #region Query Methods

        /// <inheritdoc/>
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllWithCurrentComputersAsync();
        }

        /// <inheritdoc/>
        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetWithCurrentComputersAsync(id);
        }

        #endregion
    }
}