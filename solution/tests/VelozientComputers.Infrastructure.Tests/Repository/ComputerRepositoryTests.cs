using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using VelozientComputers.Core.Entities;
using VelozientComputers.Infrastructure.Data;
using VelozientComputers.Infrastructure.Repository;

namespace VelozientComputers.Infrastructure.Tests.Repository
{
    /// <summary>
    /// Contains tests for ComputerRepository to ensure proper data access operations
    /// and complex queries involving computer assignments and status.
    /// </summary>
    public class ComputerRepositoryTests : IDisposable
    {
        #region Constants

        private static class TestData
        {
            public const int ValidComputerId = 1;
            public const int ValidUserId = 1;
            public const int ValidManufacturerId = 1;
            public const int ValidStatusId = 1;
            public const string ValidSerialNumber = "CAZ02L13ECF8";
            public const string ValidSerialNumber2 = "CAZ02L13ECF9";
            public const string ValidManufacturerName = "Apple";
            public const string ValidStatusName = "in_use";
            public const string AppleSerialRegex = "^[A-Z]{3}[C-Z0-9][1-9C-NP-RTY][A-Z0-9]{3}[A-Z0-9]{4}$";
        }

        #endregion

        #region Fields

        private readonly SqliteConnection _connection;
        private readonly ApplicationDbContext _context;
        private readonly ComputerRepository _repository;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes test context with in-memory SQLite database
        /// </summary>
        public ComputerRepositoryTests()
        {
            // Setup in-memory database
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(_connection)
                .Options;

            _context = new ApplicationDbContext(options);
            _context.Database.EnsureCreated();

            _repository = new ComputerRepository(_context);
        }

        #endregion

        #region Test Methods

        /// <summary>
        /// Tests that GetWithCurrentAssignmentAsync returns a computer with its current assignment
        /// when the computer has an active assignment.
        /// </summary>
        [Fact]
        public async Task GetWithCurrentAssignmentAsync_WithActiveAssignment_ShouldReturnComputerWithAssignment()
        {
            // Arrange
            var computer = await CreateTestComputerWithAssignment(true);

            // Act
            var result = await _repository.GetWithCurrentAssignmentAsync(computer.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.UserAssignments);
            Assert.Single(result.StatusAssignments);
            Assert.Null(result.UserAssignments.First().AssignEndDate);
            Assert.Equal(TestData.ValidUserId, result.UserAssignments.First().UserId);
        }

        /// <summary>
        /// Tests that GetWithCurrentAssignmentAsync returns a computer without active assignments
        /// when all assignments are ended.
        /// </summary>
        [Fact]
        public async Task GetWithCurrentAssignmentAsync_WithEndedAssignment_ShouldReturnComputerWithoutCurrentAssignment()
        {
            // Arrange
            var computer = await CreateTestComputerWithAssignment(false);

            // Act
            var result = await _repository.GetWithCurrentAssignmentAsync(computer.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.UserAssignments);
            Assert.Single(result.StatusAssignments);
        }

        /// <summary>
        /// Tests that GetAllWithCurrentAssignmentsAsync returns all computers with their
        /// current assignments correctly filtered.
        /// </summary>
        [Fact]
        public async Task GetAllWithCurrentAssignmentsAsync_ShouldReturnAllComputersWithCurrentAssignments()
        {
            // Arrange
            var manufacturer = await CreateTestManufacturer();
            var status = await CreateTestStatus();
            var user = await CreateTestUser();

            var computer1 = await CreateTestComputer(manufacturer.Id, TestData.ValidSerialNumber, true, user.Id, status.Id);
            var computer2 = await CreateTestComputer(manufacturer.Id, TestData.ValidSerialNumber2, false, user.Id, status.Id);

            // Act
            var results = await _repository.GetAllWithCurrentAssignmentsAsync();

            // Assert
            Assert.Equal(2, results.Count());

            var activeComputer = results.First(c => c.Id == computer1.Id);
            var inactiveComputer = results.First(c => c.Id == computer2.Id);

            Assert.Single(activeComputer.UserAssignments);
            Assert.Empty(inactiveComputer.UserAssignments);
        }

        /// <summary>
        /// Tests that RemoveAsync properly removes a computer and all its related assignments.
        /// </summary>
        [Fact]
        public async Task RemoveAsync_ShouldDeleteComputerAndRelatedAssignments()
        {
            // Arrange
            var computer = await CreateTestComputerWithAssignment(true);

            // Act
            await _repository.RemoveAsync(computer);

            // Assert
            var deletedComputer = await _context.Set<Computer>().FindAsync(computer.Id);
            Assert.Null(deletedComputer);

            var assignments = await _context.Set<ComputerUserAssignment>()
                .Where(a => a.ComputerId == computer.Id)
                .ToListAsync();
            Assert.Empty(assignments);
        }

        #endregion

        #region Helper Methods

        private async Task<Computer> CreateTestComputerWithAssignment(bool activeAssignment)
        {
            // Create manufacturer
            var manufacturer = new ComputerManufacturer
            {
                Id = TestData.ValidManufacturerId,
                Name = TestData.ValidManufacturerName,
                SerialRegex = "^[A-Z]{3}[C-Z0-9][1-9C-NP-RTY][A-Z0-9]{3}[A-Z0-9]{4}$" // Apple regex pattern
            };
            await _context.Set<ComputerManufacturer>().AddAsync(manufacturer);

            // Create status
            var status = new ComputerStatus
            {
                Id = TestData.ValidStatusId,
                LocalizedName = TestData.ValidStatusName
            };
            await _context.Set<ComputerStatus>().AddAsync(status);

            // Create user
            var user = new User
            {
                Id = TestData.ValidUserId,
                FirstName = "Test",
                LastName = "User",
                EmailAddress = "test@example.com",
                CreateDate = DateTime.UtcNow
            };
            await _context.Set<User>().AddAsync(user);

            // Create computer
            var computer = new Computer
            {
                ComputerManufacturerId = TestData.ValidManufacturerId,
                SerialNumber = TestData.ValidSerialNumber,
                PurchaseDate = DateTime.UtcNow,
                WarrantyExpirationDate = DateTime.UtcNow.AddYears(1),
                Specifications = "Test Specs",
                UserAssignments = new List<ComputerUserAssignment>(),
                StatusAssignments = new List<ComputerStatusAssignment>()
            };
            await _context.Set<Computer>().AddAsync(computer);

            // Create assignments
            var userAssignment = new ComputerUserAssignment
            {
                ComputerId = computer.Id,
                UserId = TestData.ValidUserId,
                AssignStartDate = DateTime.UtcNow.AddDays(-1),
                AssignEndDate = activeAssignment ? null : DateTime.UtcNow
            };
            computer.UserAssignments.Add(userAssignment);

            var statusAssignment = new ComputerStatusAssignment
            {
                ComputerId = computer.Id,
                ComputerStatusId = TestData.ValidStatusId,
                AssignDate = DateTime.UtcNow
            };
            computer.StatusAssignments.Add(statusAssignment);

            await _context.SaveChangesAsync();
            return computer;
        }

        private async Task<ComputerManufacturer> CreateTestManufacturer()
        {
            var manufacturer = new ComputerManufacturer
            {
                Id = TestData.ValidManufacturerId,
                Name = TestData.ValidManufacturerName,
                SerialRegex = TestData.AppleSerialRegex
            };
            await _context.Set<ComputerManufacturer>().AddAsync(manufacturer);
            await _context.SaveChangesAsync();
            return manufacturer;
        }

        private async Task<ComputerStatus> CreateTestStatus()
        {
            var status = new ComputerStatus
            {
                Id = TestData.ValidStatusId,
                LocalizedName = TestData.ValidStatusName
            };
            await _context.Set<ComputerStatus>().AddAsync(status);
            await _context.SaveChangesAsync();
            return status;
        }

        private async Task<User> CreateTestUser()
        {
            var user = new User
            {
                Id = TestData.ValidUserId,
                FirstName = "Test",
                LastName = "User",
                EmailAddress = "test@example.com",
                CreateDate = DateTime.UtcNow
            };
            await _context.Set<User>().AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        private async Task<Computer> CreateTestComputer(
            int manufacturerId,
            string serialNumber,
            bool activeAssignment,
            int userId,
            int statusId)
        {
            var computer = new Computer
            {
                ComputerManufacturerId = manufacturerId,
                SerialNumber = serialNumber,
                PurchaseDate = DateTime.UtcNow,
                WarrantyExpirationDate = DateTime.UtcNow.AddYears(1),
                Specifications = "Test Specs",
                UserAssignments = new List<ComputerUserAssignment>(),
                StatusAssignments = new List<ComputerStatusAssignment>()
            };
            await _context.Set<Computer>().AddAsync(computer);
            await _context.SaveChangesAsync();

            var userAssignment = new ComputerUserAssignment
            {
                ComputerId = computer.Id,
                UserId = userId,
                AssignStartDate = DateTime.UtcNow.AddDays(-1),
                AssignEndDate = activeAssignment ? null : DateTime.UtcNow
            };
            computer.UserAssignments.Add(userAssignment);

            var statusAssignment = new ComputerStatusAssignment
            {
                ComputerId = computer.Id,
                ComputerStatusId = statusId,
                AssignDate = DateTime.UtcNow
            };
            computer.StatusAssignments.Add(statusAssignment);

            await _context.SaveChangesAsync();
            return computer;
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            _connection?.Dispose();
            _context?.Dispose();
        }

        #endregion
    }
}