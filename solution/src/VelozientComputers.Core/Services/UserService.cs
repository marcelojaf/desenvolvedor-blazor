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
            var users = await _userRepository.GetAllWithCurrentComputersAsync();
            return users.Select(MapToUser);
        }

        /// <inheritdoc/>
        public async Task<User> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetWithCurrentComputersAsync(id);
            return user != null ? MapToUser(user) : null;
        }

        /// <inheritdoc/>
        public async Task<User> GetUserByEmailAsync(string email)
        {
            var users = await _userRepository.FindAsync(u => u.Email == email);
            var user = users.FirstOrDefault();
            return user != null ? MapToUser(user) : null;
        }

        #endregion

        #region Command Methods

        /// <inheritdoc/>
        public async Task<User> CreateUserAsync(User user)
        {
            var existingUser = await GetUserByEmailAsync(user.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("A user with this email already exists");
            }

            var user = new User
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };

            await _userRepository.AddAsync(user);
            return MapToUser(user);
        }

        /// <inheritdoc/>
        public async Task<User> UpdateUserAsync(int id, User user)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            var existingUser = await GetUserByEmailAsync(user.Email);
            if (existingUser != null && existingUser.Id != id)
            {
                throw new InvalidOperationException("A user with this email already exists");
            }

            user.FirstName = user.FirstName;
            user.LastName = user.LastName;
            user.Email = user.Email;

            _userRepository.Update(user);
            return MapToUser(user);
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

        #region Helper Methods

        /// <summary>
        /// Maps a User entity to User
        /// </summary>
        private User MapToUser(User user)
        {
            var currentAssignments = user.ComputerAssignments?
                .Where(a => a.EndDate == null)
                .Select(a => new Computer
                {
                    Id = a.Computer.Id,
                    Manufacturer = a.Computer.Manufacturer,
                    SerialNumber = a.Computer.SerialNumber,
                    Status = a.Computer.Status,
                    PurchaseDate = a.Computer.PurchaseDate,
                    WarrantyExpiryDate = a.Computer.WarrantyExpiryDate,
                    Specifications = a.Computer.Specifications,
                    ImageUrl = a.Computer.ImageUrl,
                    WarrantyStatus = GetWarrantyStatus(a.Computer.WarrantyExpiryDate)
                })
                .To();

            return new User
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                AssignedComputers = currentAssignments ?? new <Computer>()
            };
        }

        /// <summary>
        /// Determines the warranty status based on expiry date
        /// </summary>
        private string GetWarrantyStatus(DateTime warrantyExpiryDate)
        {
            var daysUntilExpiry = (warrantyExpiryDate - DateTime.UtcNow).TotalDays;

            if (daysUntilExpiry <= 0)
                return "RED";
            if (daysUntilExpiry <= 30)
                return "YELLOW";
            return "GREEN";
        }

        #endregion
    }
}