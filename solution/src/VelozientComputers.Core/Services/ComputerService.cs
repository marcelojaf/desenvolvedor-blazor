using System.Text.RegularExpressions;
using VelozientComputers.Core.Entities;
using VelozientComputers.Core.Interfaces.Repository;
using VelozientComputers.Core.Interfaces.Service;

namespace VelozientComputers.Core.Services
{
    /// <summary>
    /// Service implementation for managing computer operations
    /// </summary>
    public class ComputerService : IComputerService
    {
        #region Fields

        private readonly IComputerRepository _computerRepository;
        private readonly Dictionary<string, string> _serialNumberPatterns;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the ComputerService class
        /// </summary>
        /// <param name="computerRepository">Computer repository instance</param>
        public ComputerService(IComputerRepository computerRepository)
        {
            _computerRepository = computerRepository;
            _serialNumberPatterns = new Dictionary<string, string>
            {
                { "Apple", @"^[A-Z]{3}[C-Z0-9][1-9C-NP-RTY][A-Z0-9]{3}[A-Z0-9]{4}$" },
                { "Dell", @"^[A-Z0-9]{7}$" },
                { "HP", @"^[A-Z0-9]{3}\d{3}[A-Z0-9]{4}$" },
                { "Lenovo", @"^\d{2}-[A-Z0-9]{5}$" }
            };
        }

        #endregion

        #region Query Methods

        /// <inheritdoc/>
        public async Task<IEnumerable<Computer>> GetAllComputersAsync()
        {
            var computers = await _computerRepository.GetAllWithCurrentAssignmentsAsync();
            return computers.Select(MapToComputer);
        }

        /// <inheritdoc/>
        public async Task<Computer> GetComputerByIdAsync(int id)
        {
            var computer = await _computerRepository.GetWithCurrentAssignmentAsync(id);
            return computer != null ? MapToComputer(computer) : null;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Computer>> GetComputersWithExpiringWarrantyAsync(int daysThreshold = 30)
        {
            var expirationDate = DateTime.UtcNow.AddDays(daysThreshold);
            var computers = await _computerRepository.FindAsync(c => c.WarrantyExpiryDate <= expirationDate);
            return computers.Select(MapToComputer);
        }

        #endregion

        #region Command Methods

        /// <inheritdoc/>
        public async Task<Computer> CreateComputerAsync(Computer computer)
        {
            if (!await ValidateSerialNumberAsync(computer.Manufacturer, computer.SerialNumber))
            {
                throw new ArgumentException("Invalid serial number for the specified manufacturer");
            }

            var computer = new Computer
            {
                Manufacturer = computer.Manufacturer,
                SerialNumber = computer.SerialNumber,
                Status = computer.Status,
                PurchaseDate = computer.PurchaseDate,
                WarrantyExpiryDate = computer.WarrantyExpiryDate,
                Specifications = computer.Specifications,
                ImageUrl = computer.ImageUrl
            };

            await _computerRepository.AddAsync(computer);
            return MapToComputer(computer);
        }

        /// <inheritdoc/>
        public async Task<Computer> UpdateComputerAsync(int id, Computer computer)
        {
            var computer = await _computerRepository.GetByIdAsync(id);
            if (computer == null)
            {
                throw new KeyNotFoundException("Computer not found");
            }

            if (computer.SerialNumber != computer.SerialNumber &&
                !await ValidateSerialNumberAsync(computer.Manufacturer, computer.SerialNumber))
            {
                throw new ArgumentException("Invalid serial number for the specified manufacturer");
            }

            computer.Manufacturer = computer.Manufacturer;
            computer.SerialNumber = computer.SerialNumber;
            computer.Status = computer.Status;
            computer.PurchaseDate = computer.PurchaseDate;
            computer.WarrantyExpiryDate = computer.WarrantyExpiryDate;
            computer.Specifications = computer.Specifications;
            computer.ImageUrl = computer.ImageUrl;

            _computerRepository.Update(computer);
            return MapToComputer(computer);
        }

        /// <inheritdoc/>
        public async Task<Computer> UpdateComputerStatusAsync(int id, ComputerStatus status)
        {
            var computer = await _computerRepository.GetByIdAsync(id);
            if (computer == null)
            {
                throw new KeyNotFoundException("Computer not found");
            }

            computer.Status = status.NewStatus;
            _computerRepository.Update(computer);

            return MapToComputer(computer);
        }

        /// <inheritdoc/>
        public async Task DeleteComputerAsync(int id)
        {
            var computer = await _computerRepository.GetByIdAsync(id);
            if (computer == null)
            {
                throw new KeyNotFoundException("Computer not found");
            }

            _computerRepository.Remove(computer);
        }

        /// <inheritdoc/>
        public async Task<bool> ValidateSerialNumberAsync(string manufacturer, string serialNumber)
        {
            if (!_serialNumberPatterns.TryGetValue(manufacturer, out var pattern))
            {
                return false;
            }

            return Regex.IsMatch(serialNumber, pattern);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Maps a Computer entity to Computer
        /// </summary>
        private Computer MapToComputer(Computer computer)
        {
            var warrantyStatus = GetWarrantyStatus(computer.WarrantyExpiryDate);
            var currentAssignment = computer.Assignments
                ?.OrderByDescending(a => a.StartDate)
                .FirstOrDefault(a => a.EndDate == null);

            return new Computer
            {
                Id = computer.Id,
                Manufacturer = computer.Manufacturer,
                SerialNumber = computer.SerialNumber,
                Status = computer.Status,
                PurchaseDate = computer.PurchaseDate,
                WarrantyExpiryDate = computer.WarrantyExpiryDate,
                Specifications = computer.Specifications,
                ImageUrl = computer.ImageUrl,
                WarrantyStatus = warrantyStatus,
                CurrentUser = currentAssignment?.User != null
                    ? new User
                    {
                        Id = currentAssignment.User.Id,
                        FirstName = currentAssignment.User.FirstName,
                        LastName = currentAssignment.User.LastName,
                        Email = currentAssignment.User.Email
                    }
                    : null
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