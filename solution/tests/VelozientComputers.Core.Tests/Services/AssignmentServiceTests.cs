using Moq;
using VelozientComputers.Core.Entities;
using VelozientComputers.Core.Interfaces.Repository;
using VelozientComputers.Core.Interfaces.Service;
using VelozientComputers.Core.Services;

namespace VelozientComputers.Core.Tests.Services
{
    /// <summary>
    /// Contains unit tests for the AssignmentService class to ensure proper functionality
    /// of computer-user assignment operations.
    /// </summary>
    public class AssignmentServiceTests
    {
        #region Constants

        private static class TestData
        {
            public const int ValidComputerId = 1;
            public const int ValidUserId = 1;
            public const int ExistingAssignmentUserId = 2;
            public const string InUseStatus = "in_use";
            public const string AvailableStatus = "available";
        }

        private static class ErrorMessages
        {
            public const string ComputerAlreadyAssigned = "Computer is already assigned";
        }

        #endregion

        #region Fields

        private readonly Mock<IComputerRepository> _computerRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IStatusRepository> _statusRepositoryMock;
        private readonly IAssignmentService _assignmentService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the AssignmentServiceTests class.
        /// Sets up all necessary mocks and creates the service instance for testing.
        /// </summary>
        public AssignmentServiceTests()
        {
            _computerRepositoryMock = new Mock<IComputerRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _statusRepositoryMock = new Mock<IStatusRepository>();

            _assignmentService = new AssignmentService(
                _computerRepositoryMock.Object,
                _userRepositoryMock.Object,
                _statusRepositoryMock.Object
            );
        }

        #endregion

        #region Test Methods

        /// <summary>
        /// Tests that assigning a computer to a user succeeds when the computer is available.
        /// Verifies that the assignment is created with correct properties and the computer status is updated.
        /// </summary>
        [Fact]
        public async Task AssignComputer_ToAvailableComputer_ShouldSucceed()
        {
            // Arrange
            var computer = CreateAvailableComputer();
            var user = CreateUser();
            var inUseStatus = CreateStatus(TestData.InUseStatus);

            SetupMocksForSuccessfulAssignment(computer, user, inUseStatus);

            var assignment = new ComputerUserAssignment
            {
                ComputerId = TestData.ValidComputerId,
                UserId = TestData.ValidUserId
            };

            // Act
            var result = await _assignmentService.AssignComputerAsync(assignment);

            // Assert
            VerifySuccessfulAssignment(result);
        }

        /// <summary>
        /// Tests that attempting to assign an already assigned computer throws an InvalidOperationException.
        /// Verifies that the system properly prevents double assignments.
        /// </summary>
        [Fact]
        public async Task AssignComputer_ToAlreadyAssignedComputer_ShouldThrowException()
        {
            // Arrange
            var computer = CreateAssignedComputer();
            var user = CreateUser();

            SetupMocksForAssignedComputer(computer, user);

            var assignment = new ComputerUserAssignment
            {
                ComputerId = TestData.ValidComputerId,
                UserId = TestData.ValidUserId
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _assignmentService.AssignComputerAsync(assignment));

            Assert.Equal(ErrorMessages.ComputerAlreadyAssigned, exception.Message);
        }

        /// <summary>
        /// Tests that ending an active computer assignment succeeds.
        /// Verifies that the assignment end date is set and the computer status is updated to available.
        /// </summary>
        [Fact]
        public async Task EndAssignment_ForAssignedComputer_ShouldSucceed()
        {
            // Arrange
            var computer = CreateComputerWithActiveAssignment();
            var availableStatus = CreateStatus(TestData.AvailableStatus);
            var endDate = DateTime.UtcNow;

            SetupMocksForEndingAssignment(computer, availableStatus);

            // Act
            var result = await _assignmentService.EndAssignmentAsync(TestData.ValidComputerId, endDate);

            // Assert
            VerifySuccessfulEndAssignment(result, endDate);
        }

        #endregion

        #region Helper Methods

        private Computer CreateAvailableComputer()
        {
            return new Computer
            {
                Id = TestData.ValidComputerId,
                UserAssignments = new List<ComputerUserAssignment>(),
                StatusAssignments = new List<ComputerStatusAssignment>()
            };
        }

        private Computer CreateAssignedComputer()
        {
            return new Computer
            {
                Id = TestData.ValidComputerId,
                UserAssignments = new List<ComputerUserAssignment>
                {
                    new ComputerUserAssignment
                    {
                        ComputerId = TestData.ValidComputerId,
                        UserId = TestData.ExistingAssignmentUserId,
                        AssignStartDate = DateTime.UtcNow.AddDays(-1),
                        AssignEndDate = null
                    }
                }
            };
        }

        private Computer CreateComputerWithActiveAssignment()
        {
            return new Computer
            {
                Id = TestData.ValidComputerId,
                UserAssignments = new List<ComputerUserAssignment>
                {
                    new ComputerUserAssignment
                    {
                        ComputerId = TestData.ValidComputerId,
                        UserId = TestData.ValidUserId,
                        AssignStartDate = DateTime.UtcNow.AddDays(-1),
                        AssignEndDate = null
                    }
                },
                StatusAssignments = new List<ComputerStatusAssignment>()
            };
        }

        private User CreateUser()
        {
            return new User { Id = TestData.ValidUserId };
        }

        private ComputerStatus CreateStatus(string name)
        {
            return new ComputerStatus { Id = name == TestData.InUseStatus ? 1 : 2, LocalizedName = name };
        }

        private void SetupMocksForSuccessfulAssignment(Computer computer, User user, ComputerStatus status)
        {
            _computerRepositoryMock
                .Setup(x => x.GetWithCurrentAssignmentAsync(TestData.ValidComputerId))
                .ReturnsAsync(computer);

            _userRepositoryMock
                .Setup(x => x.GetByIdAsync(TestData.ValidUserId))
                .ReturnsAsync(user);

            _statusRepositoryMock
                .Setup(x => x.GetByNameAsync(TestData.InUseStatus))
                .ReturnsAsync(status);

            _computerRepositoryMock
                .Setup(x => x.UpdateAsync(It.IsAny<Computer>()))
                .Returns(Task.CompletedTask);
        }

        private void SetupMocksForAssignedComputer(Computer computer, User user)
        {
            _computerRepositoryMock
                .Setup(x => x.GetWithCurrentAssignmentAsync(TestData.ValidComputerId))
                .ReturnsAsync(computer);

            _userRepositoryMock
                .Setup(x => x.GetByIdAsync(TestData.ValidUserId))
                .ReturnsAsync(user);
        }

        private void SetupMocksForEndingAssignment(Computer computer, ComputerStatus status)
        {
            _computerRepositoryMock
                .Setup(x => x.GetWithCurrentAssignmentAsync(TestData.ValidComputerId))
                .ReturnsAsync(computer);

            _statusRepositoryMock
                .Setup(x => x.GetByNameAsync(TestData.AvailableStatus))
                .ReturnsAsync(status);

            _computerRepositoryMock
                .Setup(x => x.UpdateAsync(It.IsAny<Computer>()))
                .Returns(Task.CompletedTask);
        }

        private void VerifySuccessfulAssignment(ComputerUserAssignment result)
        {
            Assert.NotNull(result);
            Assert.Equal(TestData.ValidComputerId, result.ComputerId);
            Assert.Equal(TestData.ValidUserId, result.UserId);
            Assert.Null(result.AssignEndDate);
            _computerRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Computer>()), Times.Once);
        }

        private void VerifySuccessfulEndAssignment(ComputerUserAssignment result, DateTime endDate)
        {
            Assert.NotNull(result);
            Assert.NotNull(result.AssignEndDate);
            Assert.Equal(TestData.ValidComputerId, result.ComputerId);
            Assert.Equal(TestData.ValidUserId, result.UserId);
            Assert.Equal(endDate, result.AssignEndDate);
            _computerRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Computer>()), Times.Once);
        }

        #endregion
    }
}