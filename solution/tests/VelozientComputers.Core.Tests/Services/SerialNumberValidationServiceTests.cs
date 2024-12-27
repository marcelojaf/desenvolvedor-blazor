using Moq;
using VelozientComputers.Core.Entities;
using VelozientComputers.Core.Interfaces.Repository;
using VelozientComputers.Core.Interfaces.Service;
using VelozientComputers.Core.Services;

namespace VelozientComputers.Core.Tests.Services
{
    public class SerialNumberValidationServiceTests
    {
        private readonly Mock<IManufacturerRepository> _manufacturerRepositoryMock;
        private readonly Mock<IComputerRepository> _computerRepositoryMock;
        private readonly ISerialNumberValidationService _validationService;

        public SerialNumberValidationServiceTests()
        {
            _manufacturerRepositoryMock = new Mock<IManufacturerRepository>();
            _computerRepositoryMock = new Mock<IComputerRepository>();

            _validationService = new SerialNumberValidationService(
                _manufacturerRepositoryMock.Object,
                _computerRepositoryMock.Object
            );
        }

        [Theory]
        [InlineData("CAZ02L13ECF8", 1, true)] // Valid Apple serial
        [InlineData("A1B2C3D", 2, true)]      // Valid Dell serial
        [InlineData("ABC123DEF4", 3, true)]   // Valid HP serial
        [InlineData("12-AB123", 4, true)]     // Valid Lenovo serial
        [InlineData("INVALID", 1, false)]      // Invalid serial
        public async Task ValidateSerialNumber_ShouldReturnCorrectResult(string serialNumber, int manufacturerId, bool expectedIsValid)
        {
            // Arrange
            var manufacturer = new ComputerManufacturer { Id = manufacturerId };
            _manufacturerRepositoryMock
                .Setup(x => x.GetByIdAsync(manufacturerId))
                .ReturnsAsync(manufacturer);

            // Act
            var (isValid, _) = await _validationService.ValidateSerialNumberAsync(serialNumber, manufacturerId);

            // Assert
            Assert.Equal(expectedIsValid, isValid);
        }

        [Fact]
        public async Task IsSerialNumberUnique_WithExistingSerial_ShouldReturnFalse()
        {
            // Arrange
            var serialNumber = "ABC123DEF4";
            _computerRepositoryMock
                .Setup(x => x.ExistsAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Computer, bool>>>()))
                .ReturnsAsync(true);

            // Act
            var result = await _validationService.IsSerialNumberUniqueAsync(serialNumber);

            // Assert
            Assert.False(result);
        }
    }
}
