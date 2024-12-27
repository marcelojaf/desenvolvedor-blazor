using Moq;
using VelozientComputers.Core.Entities;
using VelozientComputers.Core.Interfaces.Repository;
using VelozientComputers.Core.Interfaces.Service;
using VelozientComputers.Core.Services;

namespace VelozientComputers.Core.Tests.Services
{
    /// <summary>
    /// Contains unit tests for the ComputerService class to ensure proper functionality
    /// of computer management operations including creation, validation, and warranty tracking.
    /// </summary>
    public class ComputerServiceTests
    {
        #region Constants

        private static class TestData
        {
            public const int ValidManufacturerId = 1;
            public const string ValidSerialNumber = "ABC123DEF4";
            public const string ValidSpecifications = "Test Specs";
            public const int WarrantyThresholdDays = 30;
        }

        private static class ErrorMessages
        {
            public const string DuplicateSerialNumber = "Serial number must be unique";
        }

        #endregion

        #region Fields

        private readonly Mock<IComputerRepository> _computerRepositoryMock;
        private readonly Mock<IManufacturerRepository> _manufacturerRepositoryMock;
        private readonly Mock<ISerialNumberValidationService> _serialValidationServiceMock;
        private readonly IComputerService _computerService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the ComputerServiceTests class.
        /// Sets up all necessary mocks and creates the service instance for testing.
        /// </summary>
        public ComputerServiceTests()
        {
            _computerRepositoryMock = new Mock<IComputerRepository>();
            _manufacturerRepositoryMock = new Mock<IManufacturerRepository>();
            _serialValidationServiceMock = new Mock<ISerialNumberValidationService>();

            _computerService = new ComputerService(
                _computerRepositoryMock.Object,
                _manufacturerRepositoryMock.Object,
                _serialValidationServiceMock.Object
            );
        }

        #endregion

        #region Test Methods

        /// <summary>
        /// Tests that creating a computer with valid data succeeds.
        /// Verifies that the computer is created with correct properties and proper validation is performed.
        /// </summary>
        [Fact]
        public async Task CreateComputer_WithValidData_ShouldSucceed()
        {
            // Arrange
            var computer = CreateValidComputer();
            SetupMocksForValidComputerCreation();

            // Act
            var result = await _computerService.CreateComputerAsync(computer);

            // Assert
            VerifySuccessfulComputerCreation(result);
        }

        /// <summary>
        /// Tests that attempting to create a computer with a duplicate serial number throws ArgumentException.
        /// Verifies that the system properly prevents duplicate serial numbers.
        /// </summary>
        [Fact]
        public async Task CreateComputer_WithDuplicateSerialNumber_ShouldThrowException()
        {
            // Arrange
            var computer = CreateValidComputer();
            SetupMocksForDuplicateSerialNumber();

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => _computerService.CreateComputerAsync(computer));

            Assert.Equal(ErrorMessages.DuplicateSerialNumber, exception.Message);
        }

        /// <summary>
        /// Tests that the system correctly identifies computers with warranties expiring soon.
        /// Verifies that only computers within the threshold are returned.
        /// </summary>
        [Fact]
        public async Task GetComputersWithExpiringWarranty_ShouldReturnCorrectComputers()
        {
            // Arrange
            var computers = CreateComputerListWithMixedWarranties();
            SetupMocksForWarrantyCheck(computers);

            // Act
            var result = await _computerService.GetComputersWithExpiringWarrantyAsync(TestData.WarrantyThresholdDays);

            // Assert
            VerifyExpiringWarrantyResults(result);
        }

        #endregion

        #region Helper Methods

        private Computer CreateValidComputer()
        {
            return new Computer
            {
                ComputerManufacturerId = TestData.ValidManufacturerId,
                SerialNumber = TestData.ValidSerialNumber,
                PurchaseDate = DateTime.UtcNow,
                WarrantyExpirationDate = DateTime.UtcNow.AddYears(1),
                Specifications = TestData.ValidSpecifications
            };
        }

        private List<Computer> CreateComputerListWithMixedWarranties()
        {
            return new List<Computer>
            {
                new Computer
                {
                    Id = 1,
                    WarrantyExpirationDate = DateTime.UtcNow.AddDays(15)
                },
                new Computer
                {
                    Id = 2,
                    WarrantyExpirationDate = DateTime.UtcNow.AddDays(45)
                }
            };
        }

        private void SetupMocksForValidComputerCreation()
        {
            _serialValidationServiceMock
                .Setup(x => x.ValidateSerialNumberAsync(It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync((true, string.Empty));

            _serialValidationServiceMock
                .Setup(x => x.IsSerialNumberUniqueAsync(It.IsAny<string>(), null))
                .ReturnsAsync(true);

            _computerRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Computer>()))
                .Returns(Task.CompletedTask);
        }

        private void SetupMocksForDuplicateSerialNumber()
        {
            _serialValidationServiceMock
                .Setup(x => x.ValidateSerialNumberAsync(It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync((true, string.Empty));

            _serialValidationServiceMock
                .Setup(x => x.IsSerialNumberUniqueAsync(It.IsAny<string>(), null))
                .ReturnsAsync(false);
        }

        private void SetupMocksForWarrantyCheck(List<Computer> computers)
        {
            _computerRepositoryMock
                .Setup(x => x.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Computer, bool>>>()))
                .ReturnsAsync(computers.Where(c =>
                    c.WarrantyExpirationDate <= DateTime.UtcNow.AddDays(TestData.WarrantyThresholdDays)));
        }

        private void VerifySuccessfulComputerCreation(Computer result)
        {
            Assert.NotNull(result);
            Assert.Equal(TestData.ValidSerialNumber, result.SerialNumber);
            _computerRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Computer>()), Times.Once);
        }

        private void VerifyExpiringWarrantyResults(IEnumerable<Computer> result)
        {
            Assert.Single(result);
            Assert.Equal(1, result.First().Id);
        }

        #endregion
    }
}