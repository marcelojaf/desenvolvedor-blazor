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

        /// <inheritdoc/>
        public async Task<User> GetUserByEmailAsync(string email)
        {
            var users = await _userRepository.FindAsync(u => u.EmailAddress == email);
            return users.FirstOrDefault();
        }

        #endregion

        #region Command Methods

        /// <inheritdoc/>
        public async Task<User> CreateUserAsync(User user)
        {
            var existingUser = await GetUserByEmailAsync(user.EmailAddress);
            if (existingUser != null)
            {
                throw new InvalidOperationException("A user with this email already exists");
            }

            await _userRepository.AddAsync(user);
            return user;
        }

        /// <inheritdoc/>
        public async Task<User> UpdateUserAsync(int id, User updatedUser)
        {
            var existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            var userWithEmail = await GetUserByEmailAsync(updatedUser.EmailAddress);
            if (userWithEmail != null && userWithEmail.Id != id)
            {
                throw new InvalidOperationException("A user with this email already exists");
            }

            existingUser.FirstName = updatedUser.FirstName;
            existingUser.LastName = updatedUser.LastName;
            existingUser.EmailAddress = updatedUser.EmailAddress;

            _userRepository.Update(existingUser);
            return existingUser;
        }

        /// <inheritdoc/>
        public async Task DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            _userRepository.Remove(user);
        }

        #endregion
    }
}