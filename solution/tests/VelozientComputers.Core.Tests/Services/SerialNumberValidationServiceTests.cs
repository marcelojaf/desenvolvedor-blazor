using Moq;
using VelozientComputers.Core.Entities;
using VelozientComputers.Core.Interfaces.Repository;
using VelozientComputers.Core.Interfaces.Service;
using VelozientComputers.Core.Services;

namespace VelozientComputers.Core.Tests.Services
{
    /// <summary>
    /// Contains unit tests for the SerialNumberValidationService class to ensure
    /// proper validation of computer serial numbers according to manufacturer-specific formats.
    /// </summary>
    public class SerialNumberValidationServiceTests
    {
        #region Constants

        private static class TestData
        {
            public const string ValidAppleSerial = "CAZ02L13ECF8";
            public const string ValidDellSerial = "A1B2C3D";
            public const string ValidHpSerial = "ABC123DEF4";
            public const string ValidLenovoSerial = "12-AB123";
            public const string InvalidSerial = "INVALID";
            public const string ExistingSerial = "ABC123DEF4";

            public const int AppleManufacturerId = 1;
            public const int DellManufacturerId = 2;
            public const int HpManufacturerId = 3;
            public const int LenovoManufacturerId = 4;
        }

        #endregion

        #region Fields

        private readonly Mock<IManufacturerRepository> _manufacturerRepositoryMock;
        private readonly Mock<IComputerRepository> _computerRepositoryMock;
        private readonly ISerialNumberValidationService _validationService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the SerialNumberValidationServiceTests class.
        /// Sets up all necessary mocks and creates the service instance for testing.
        /// </summary>
        public SerialNumberValidationServiceTests()
        {
            _manufacturerRepositoryMock = new Mock<IManufacturerRepository>();
            _computerRepositoryMock = new Mock<IComputerRepository>();

            _validationService = new SerialNumberValidationService(
                _manufacturerRepositoryMock.Object,
                _computerRepositoryMock.Object
            );
        }

        #endregion

        #region Test Methods

        /// <summary>
        /// Tests the validation of serial numbers for different manufacturers.
        /// Verifies that the system correctly validates both valid and invalid formats.
        /// </summary>
        /// <param name="serialNumber">The serial number to validate</param>
        /// <param name="manufacturerId">The ID of the manufacturer</param>
        /// <param name="expectedIsValid">Whether the serial number should be considered valid</param>
        [Theory]
        [InlineData(TestData.ValidAppleSerial, TestData.AppleManufacturerId, true)]
        [InlineData(TestData.ValidDellSerial, TestData.DellManufacturerId, true)]
        [InlineData(TestData.ValidHpSerial, TestData.HpManufacturerId, true)]
        [InlineData(TestData.ValidLenovoSerial, TestData.LenovoManufacturerId, true)]
        [InlineData(TestData.InvalidSerial, TestData.AppleManufacturerId, false)]
        public async Task ValidateSerialNumber_ShouldReturnCorrectResult(
            string serialNumber,
            int manufacturerId,
            bool expectedIsValid)
        {
            // Arrange
            SetupManufacturerMock(manufacturerId);

            // Act
            var (isValid, _) = await _validationService.ValidateSerialNumberAsync(serialNumber, manufacturerId);

            // Assert
            Assert.Equal(expectedIsValid, isValid);
        }

        /// <summary>
        /// Tests that the system correctly identifies duplicate serial numbers.
        /// Verifies that existing serial numbers are properly detected.
        /// </summary>
        [Fact]
        public async Task IsSerialNumberUnique_WithExistingSerial_ShouldReturnFalse()
        {
            // Arrange
            SetupExistingSerialNumberCheck();

            // Act
            var result = await _validationService.IsSerialNumberUniqueAsync(TestData.ExistingSerial);

            // Assert
            Assert.False(result);
            VerifySerialNumberCheck();
        }

        #endregion

        #region Helper Methods

        private void SetupManufacturerMock(int manufacturerId)
        {
            _manufacturerRepositoryMock
                .Setup(x => x.GetByIdAsync(manufacturerId))
                .ReturnsAsync(new ComputerManufacturer { Id = manufacturerId });
        }

        private void SetupExistingSerialNumberCheck()
        {
            _computerRepositoryMock
                .Setup(x => x.ExistsAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Computer, bool>>>()))
                .ReturnsAsync(true);
        }

        private void VerifySerialNumberCheck()
        {
            _computerRepositoryMock.Verify(
                x => x.ExistsAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Computer, bool>>>()),
                Times.Once);
        }

        #endregion
    }
}