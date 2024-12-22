using System.Text.RegularExpressions;
using VelozientComputers.Core.Entities;
using VelozientComputers.Infrastructure.Repository;
using VelozientComputers.Api.DTOs;

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
        public async Task<IEnumerable<ComputerListDto>> GetAllComputersAsync()
        {
            var computers = await _computerRepository.GetAllWithCurrentAssignmentsAsync();
            return computers.Select(MapToComputerListDto);
        }

        /// <inheritdoc/>
        public async Task<ComputerListDto> GetComputerByIdAsync(int id)
        {
            var computer = await _computerRepository.GetWithCurrentAssignmentAsync(id);
            return computer != null ? MapToComputerListDto(computer) : null;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ComputerListDto>> GetComputersWithExpiringWarrantyAsync(int daysThreshold = 30)
        {
            var expirationDate = DateTime.UtcNow.AddDays(daysThreshold);
            var computers = await _computerRepository.FindAsync(c => c.WarrantyExpiryDate <= expirationDate);
            return computers.Select(MapToComputerListDto);
        }

        #endregion

        #region Command Methods

        /// <inheritdoc/>
        public async Task<ComputerListDto> CreateComputerAsync(CreateComputerDto computerDto)
        {
            if (!await ValidateSerialNumberAsync(computerDto.Manufacturer, computerDto.SerialNumber))
            {
                throw new ArgumentException("Invalid serial number for the specified manufacturer");
            }

            var computer = new Computer
            {
                Manufacturer = computerDto.Manufacturer,
                SerialNumber = computerDto.SerialNumber,
                Status = computerDto.Status,
                PurchaseDate = computerDto.PurchaseDate,
                WarrantyExpiryDate = computerDto.WarrantyExpiryDate,
                Specifications = computerDto.Specifications,
                ImageUrl = computerDto.ImageUrl
            };

            await _computerRepository.AddAsync(computer);
            return MapToComputerListDto(computer);
        }

        /// <inheritdoc/>
        public async Task<ComputerListDto> UpdateComputerAsync(int id, UpdateComputerDto computerDto)
        {
            var computer = await _computerRepository.GetByIdAsync(id);
            if (computer == null)
            {
                throw new KeyNotFoundException("Computer not found");
            }

            if (computerDto.SerialNumber != computer.SerialNumber && 
                !await ValidateSerialNumberAsync(computerDto.Manufacturer, computerDto.SerialNumber))
            {
                throw new ArgumentException("Invalid serial number for the specified manufacturer");
            }

            computer.Manufacturer = computerDto.Manufacturer;
            computer.SerialNumber = computerDto.SerialNumber;
            computer.Status = computerDto.Status;
            computer.PurchaseDate = computerDto.PurchaseDate;
            computer.WarrantyExpiryDate = computerDto.WarrantyExpiryDate;
            computer.Specifications = computerDto.Specifications;
            computer.ImageUrl = computerDto.ImageUrl;

            _computerRepository.Update(computer);
            return MapToComputerListDto(computer);
        }

        /// <inheritdoc/>
        public async Task<ComputerListDto> UpdateComputerStatusAsync(int id, UpdateComputerStatusDto statusDto)
        {
            var computer = await _computerRepository.GetByIdAsync(id);
            if (computer == null)
            {
                throw new KeyNotFoundException("Computer not found");
            }

            computer.Status = statusDto.NewStatus;
            _computerRepository.Update(computer);
            
            return MapToComputerListDto(computer);
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
        /// Maps a Computer entity to ComputerListDto
        /// </summary>
        private ComputerListDto MapToComputerListDto(Computer computer)
        {
            var warrantyStatus = GetWarrantyStatus(computer.WarrantyExpiryDate);
            var currentAssignment = computer.Assignments
                ?.OrderByDescending(a => a.StartDate)
                .FirstOrDefault(a => a.EndDate == null);

            return new ComputerListDto
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
                    ? new UserListDto 
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