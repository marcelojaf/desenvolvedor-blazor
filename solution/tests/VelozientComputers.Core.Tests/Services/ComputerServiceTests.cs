using Moq;
using VelozientComputers.Core.Entities;
using VelozientComputers.Core.Interfaces.Repository;
using VelozientComputers.Core.Interfaces.Service;
using VelozientComputers.Core.Services;

namespace VelozientComputers.Core.Tests.Services
{
    public class ComputerServiceTests
    {
        private readonly Mock<IComputerRepository> _computerRepositoryMock;
        private readonly Mock<IManufacturerRepository> _manufacturerRepositoryMock;
        private readonly Mock<ISerialNumberValidationService> _serialValidationServiceMock;
        private readonly IComputerService _computerService;

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

        [Fact]
        public async Task CreateComputer_WithValidData_ShouldSucceed()
        {
            // Arrange
            var computer = new Computer
            {
                ComputerManufacturerId = 1,
                SerialNumber = "ABC123DEF4",
                PurchaseDate = DateTime.UtcNow,
                WarrantyExpirationDate = DateTime.UtcNow.AddYears(1),
                Specifications = "Test Specs"
            };

            _serialValidationServiceMock
                .Setup(x => x.ValidateSerialNumberAsync(It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync((true, string.Empty));

            _serialValidationServiceMock
                .Setup(x => x.IsSerialNumberUniqueAsync(It.IsAny<string>(), null))
                .ReturnsAsync(true);

            _computerRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Computer>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _computerService.CreateComputerAsync(computer);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(computer.SerialNumber, result.SerialNumber);
            _computerRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Computer>()), Times.Once);
        }

        [Fact]
        public async Task CreateComputer_WithDuplicateSerialNumber_ShouldThrowException()
        {
            // Arrange
            var computer = new Computer
            {
                ComputerManufacturerId = 1,
                SerialNumber = "ABC123DEF4"
            };

            _serialValidationServiceMock
                .Setup(x => x.ValidateSerialNumberAsync(It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync((true, string.Empty));

            _serialValidationServiceMock
                .Setup(x => x.IsSerialNumberUniqueAsync(It.IsAny<string>(), null))
                .ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _computerService.CreateComputerAsync(computer));
        }

        [Fact]
        public async Task GetComputersWithExpiringWarranty_ShouldReturnCorrectComputers()
        {
            // Arrange
            var daysThreshold = 30;
            var computers = new List<Computer>
            {
                new Computer { Id = 1, WarrantyExpirationDate = DateTime.UtcNow.AddDays(15) },
                new Computer { Id = 2, WarrantyExpirationDate = DateTime.UtcNow.AddDays(45) }
            };

            _computerRepositoryMock
                .Setup(x => x.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Computer, bool>>>()))
                .ReturnsAsync(computers.Where(c => c.WarrantyExpirationDate <= DateTime.UtcNow.AddDays(daysThreshold)));

            // Act
            var result = await _computerService.GetComputersWithExpiringWarrantyAsync(daysThreshold);

            // Assert
            Assert.Single(result);
            Assert.Equal(1, result.First().Id);
        }
    }
}
