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

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the AssignmentService class
        /// </summary>
        /// <param name="computerRepository">Computer repository instance</param>
        /// <param name="userRepository">User repository instance</param>
        public AssignmentService(IComputerRepository computerRepository, IUserRepository userRepository)
        {
            _computerRepository = computerRepository;
            _userRepository = userRepository;
        }

        #endregion

        #region Query Methods

        /// <inheritdoc/>
        public async Task<ComputerAssignment> GetCurrentAssignmentAsync(int computerId)
        {
            var computer = await _computerRepository.GetWithCurrentAssignmentAsync(computerId);
            if (computer == null)
            {
                throw new KeyNotFoundException("Computer not found");
            }

            var currentAssignment = computer.Assignments?
                .OrderByDescending(a => a.StartDate)
                .FirstOrDefault(a => a.EndDate == null);

            return currentAssignment != null ? MapToAssignment(currentAssignment) : null;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ComputerAssignment>> GetComputerAssignmentHistoryAsync(int computerId)
        {
            var computer = await _computerRepository.GetWithCurrentAssignmentAsync(computerId);
            if (computer == null)
            {
                throw new KeyNotFoundException("Computer not found");
            }

            return computer.Assignments?
                .OrderByDescending(a => a.StartDate)
                .Select(MapToAssignment) ?? Enumerable.Empty<ComputerAssignment>();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ComputerAssignment>> GetUserAssignmentHistoryAsync(int userId)
        {
            var user = await _userRepository.GetWithCurrentComputersAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            return user.ComputerAssignments?
                .OrderByDescending(a => a.AssignStartDate)
                .Select(MapToAssignment) ?? Enumerable.Empty<ComputerAssignment>();
        }

        #endregion

        #region Command Methods

        /// <inheritdoc/>
        public async Task<ComputerAssignment> AssignComputerAsync(ComputerAssignment assignment)
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

            var currentAssignment = computer.Assignments?
                .OrderByDescending(a => a.StartDate)
                .FirstOrDefault(a => a.EndDate == null);

            if (currentAssignment != null)
            {
                throw new InvalidOperationException("Computer is already assigned");
            }

            var assignment = new ComputerAssignment
            {
                ComputerId = assignment.ComputerId,
                UserId = assignment.UserId,
                StartDate = assignment.StartDate,
                EndDate = assignment.EndDate
            };

            computer.Status = "In Use";
            computer.Assignments ??= new List<ComputerAssignment>();
            computer.Assignments.Add(assignment);

            _computerRepository.Update(computer);

            return MapToAssignment(assignment);
        }

        /// <inheritdoc/>
        public async Task<ComputerAssignment> EndAssignmentAsync(int computerId, DateTime endDate)
        {
            var computer = await _computerRepository.GetWithCurrentAssignmentAsync(computerId);
            if (computer == null)
            {
                throw new KeyNotFoundException("Computer not found");
            }

            var currentAssignment = computer.Assignments?
                .OrderByDescending(a => a.StartDate)
                .FirstOrDefault(a => a.EndDate == null);

            if (currentAssignment == null)
            {
                throw new InvalidOperationException("Computer is not currently assigned");
            }

            currentAssignment.EndDate = endDate;
            computer.Status = "Available";

            _computerRepository.Update(computer);

            return MapToAssignment(currentAssignment);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Maps a ComputerAssignment entity to ComputerAssignment
        /// </summary>
        private ComputerAssignment MapToAssignment(ComputerAssignment assignment)
        {
            return new ComputerAssignment
            {
                ComputerId = assignment.ComputerId,
                UserId = assignment.UserId,
                AssignmentStartDate = assignment.AssignmentStartDate,
                AssignmentEndDate = assignment.AssignmentEndDate
            };
        }

        #endregion
    }
}