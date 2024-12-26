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
        private readonly IManufacturerRepository _manufacturerRepository;
        private readonly ISerialNumberValidationService _serialNumberValidationService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the ComputerService class
        /// </summary>
        /// <param name="computerRepository">Computer repository instance</param>
        /// <param name="manufacturerRepository">Manufacturer repository instance</param>
        /// <param name="serialNumberValidationService">Serial number validation service</param>
        public ComputerService(
            IComputerRepository computerRepository,
            IManufacturerRepository manufacturerRepository,
            ISerialNumberValidationService serialNumberValidationService)
        {
            _computerRepository = computerRepository;
            _manufacturerRepository = manufacturerRepository;
            _serialNumberValidationService = serialNumberValidationService;
        }

        #endregion

        #region Query Methods

        /// <inheritdoc/>
        public async Task<IEnumerable<Computer>> GetAllComputersAsync()
        {
            return await _computerRepository.GetAllWithCurrentAssignmentsAsync();
        }

        /// <inheritdoc/>
        public async Task<Computer> GetComputerByIdAsync(int id)
        {
            return await _computerRepository.GetWithCurrentAssignmentAsync(id);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Computer>> GetComputersWithExpiringWarrantyAsync(int daysThreshold = 30)
        {
            var expirationDate = DateTime.UtcNow.AddDays(daysThreshold);
            return await _computerRepository.FindAsync(c => c.WarrantyExpirationDate <= expirationDate);
        }

        #endregion

        #region Command Methods

        /// <inheritdoc/>
        public async Task<Computer> CreateComputerAsync(Computer computer)
        {
            var (isValid, errorMessage) = await _serialNumberValidationService
                .ValidateSerialNumberAsync(computer.SerialNumber, computer.ComputerManufacturerId);

            if (!isValid)
            {
                throw new ArgumentException(errorMessage);
            }

            await _computerRepository.AddAsync(computer);
            return computer;
        }

        /// <inheritdoc/>
        public async Task<Computer> UpdateComputerAsync(int id, Computer updatedComputer)
        {
            var existingComputer = await _computerRepository.GetByIdAsync(id);
            if (existingComputer == null)
            {
                throw new KeyNotFoundException("Computer not found");
            }

            if (existingComputer.SerialNumber != updatedComputer.SerialNumber ||
                existingComputer.ComputerManufacturerId != updatedComputer.ComputerManufacturerId)
            {
                var (isValid, errorMessage) = await _serialNumberValidationService
                    .ValidateSerialNumberAsync(updatedComputer.SerialNumber, updatedComputer.ComputerManufacturerId);

                if (!isValid)
                {
                    throw new ArgumentException(errorMessage);
                }
            }

            existingComputer.ComputerManufacturerId = updatedComputer.ComputerManufacturerId;
            existingComputer.SerialNumber = updatedComputer.SerialNumber;
            existingComputer.PurchaseDate = updatedComputer.PurchaseDate;
            existingComputer.WarrantyExpirationDate = updatedComputer.WarrantyExpirationDate;
            existingComputer.Specifications = updatedComputer.Specifications;
            existingComputer.ImageUrl = updatedComputer.ImageUrl;

            await _computerRepository.UpdateAsync(existingComputer);
            return existingComputer;
        }

        /// <inheritdoc/>
        public async Task<Computer> UpdateComputerStatusAsync(int id, ComputerStatusAssignment statusAssignment)
        {
            var computer = await _computerRepository.GetByIdAsync(id);
            if (computer == null)
            {
                throw new KeyNotFoundException("Computer not found");
            }

            computer.StatusAssignments.Add(statusAssignment);
            await _computerRepository.UpdateAsync(computer);

            return computer;
        }

        /// <inheritdoc/>
        public async Task DeleteComputerAsync(int id)
        {
            var computer = await _computerRepository.GetByIdAsync(id);
            if (computer == null)
            {
                throw new KeyNotFoundException("Computer not found");
            }

            await _computerRepository.RemoveAsync(computer);
        }

        /// <inheritdoc/>
        public async Task<bool> ValidateSerialNumberAsync(string manufacturer, string serialNumber)
        {
            var manufacturerEntity = await _manufacturerRepository.FindAsync(m => m.Name == manufacturer);
            var manufacturerId = manufacturerEntity.FirstOrDefault()?.Id ?? 0;

            if (manufacturerId == 0)
            {
                return false;
            }

            var (isValid, _) = await _serialNumberValidationService.ValidateSerialNumberAsync(serialNumber, manufacturerId);
            return isValid;
        }

        #endregion
    }
}