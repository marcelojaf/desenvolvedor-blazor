using VelozientComputers.Core.Entities;
using VelozientComputers.Core.Interfaces.Repository;
using VelozientComputers.Core.Interfaces.Service;

namespace VelozientComputers.Core.Services
{
    /// <summary>
    /// Service implementation for managing computer assignments
    /// </summary>
    public class AssignmentService : IAssignmentService
    {
        #region Fields

        private readonly IComputerRepository _computerRepository;
        private readonly IUserRepository _userRepository;
        private readonly IStatusRepository _statusRepository;

        #endregion

        #region Constants

        private static class ComputerStatus
        {
            public const string InUse = "in_use";
            public const string Available = "available";
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the AssignmentService class
        /// </summary>
        /// <param name="computerRepository">Computer repository instance</param>
        /// <param name="userRepository">User repository instance</param>
        /// <param name="statusRepository">Status repository instance</param>
        public AssignmentService(
            IComputerRepository computerRepository,
            IUserRepository userRepository,
            IStatusRepository statusRepository)
        {
            _computerRepository = computerRepository;
            _userRepository = userRepository;
            _statusRepository = statusRepository;
        }

        #endregion

        #region Query Methods

        /// <inheritdoc/>
        public async Task<ComputerUserAssignment> GetCurrentAssignmentAsync(int computerId)
        {
            var computer = await _computerRepository.GetWithCurrentAssignmentAsync(computerId);
            if (computer == null)
            {
                throw new KeyNotFoundException("Computer not found");
            }

            var currentAssignment = computer.UserAssignments?
                .OrderByDescending(a => a.AssignStartDate)
                .FirstOrDefault(a => a.AssignEndDate == null);

            return currentAssignment;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ComputerUserAssignment>> GetComputerAssignmentHistoryAsync(int computerId)
        {
            var computer = await _computerRepository.GetWithCurrentAssignmentAsync(computerId);
            if (computer == null)
            {
                throw new KeyNotFoundException("Computer not found");
            }

            return computer.UserAssignments?
                .OrderByDescending(a => a.AssignStartDate)
                ?? Enumerable.Empty<ComputerUserAssignment>();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ComputerUserAssignment>> GetUserAssignmentHistoryAsync(int userId)
        {
            var user = await _userRepository.GetWithCurrentComputersAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            return user.ComputerAssignments?
                .OrderByDescending(a => a.AssignStartDate)
                ?? Enumerable.Empty<ComputerUserAssignment>();
        }

        #endregion

        #region Command Methods

        /// <inheritdoc/>
        public async Task<ComputerUserAssignment> AssignComputerAsync(ComputerUserAssignment assignment)
        {
            var computer = await _computerRepository.GetWithCurrentAssignmentAsync(assignment.ComputerId);
            if (computer == null)
            {
                throw new KeyNotFoundException("Computer not found");
            }

            var user = await _userRepository.GetByIdAsync(assignment.UserId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            var currentAssignment = computer.UserAssignments?
                .OrderByDescending(a => a.AssignStartDate)
                .FirstOrDefault(a => a.AssignEndDate == null);

            if (currentAssignment != null)
            {
                throw new InvalidOperationException("Computer is already assigned");
            }

            var newAssignment = new ComputerUserAssignment
            {
                ComputerId = assignment.ComputerId,
                UserId = assignment.UserId,
                AssignStartDate = DateTime.UtcNow,
                Computer = computer,
                User = user
            };

            var inUseStatus = await _statusRepository.GetByNameAsync(ComputerStatus.InUse);
            if (inUseStatus == null)
            {
                throw new InvalidOperationException("Status 'In Use' not found");
            }

            var statusAssignment = new ComputerStatusAssignment
            {
                ComputerId = computer.Id,
                ComputerStatusId = inUseStatus.Id,
                AssignDate = DateTime.UtcNow
            };

            computer.UserAssignments ??= new List<ComputerUserAssignment>();
            computer.UserAssignments.Add(newAssignment);
            computer.StatusAssignments.Add(statusAssignment);

            await _computerRepository.UpdateAsync(computer);

            return newAssignment;
        }

        /// <inheritdoc/>
        public async Task<ComputerUserAssignment> EndAssignmentAsync(int computerId, DateTime endDate)
        {
            var computer = await _computerRepository.GetWithCurrentAssignmentAsync(computerId);
            if (computer == null)
            {
                throw new KeyNotFoundException("Computer not found");
            }

            var currentAssignment = computer.UserAssignments?
                .OrderByDescending(a => a.AssignStartDate)
                .FirstOrDefault(a => a.AssignEndDate == null);

            if (currentAssignment == null)
            {
                throw new InvalidOperationException("Computer is not currently assigned");
            }

            currentAssignment.AssignEndDate = endDate;

            var availableStatus = await _statusRepository.GetByNameAsync(ComputerStatus.Available);
            if (availableStatus == null)
            {
                throw new InvalidOperationException("Status 'Available' not found");
            }

            var statusAssignment = new ComputerStatusAssignment
            {
                ComputerId = computer.Id,
                ComputerStatusId = availableStatus.Id,
                AssignDate = endDate
            };

            computer.StatusAssignments.Add(statusAssignment);

            await _computerRepository.UpdateAsync(computer);

            return currentAssignment;
        }

        #endregion
    }
}