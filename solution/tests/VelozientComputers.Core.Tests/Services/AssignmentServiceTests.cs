using Moq;
using VelozientComputers.Core.Entities;
using VelozientComputers.Core.Interfaces.Repository;
using VelozientComputers.Core.Interfaces.Service;
using VelozientComputers.Core.Services;

namespace VelozientComputers.Core.Tests.Services
{
    public class AssignmentServiceTests
    {
        private readonly Mock<IComputerRepository> _computerRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IStatusRepository> _statusRepositoryMock;
        private readonly IAssignmentService _assignmentService;

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

        [Fact]
        public async Task AssignComputer_ToAvailableComputer_ShouldSucceed()
        {
            // Arrange
            var computerId = 1;
            var userId = 1;
            var computer = new Computer
            {
                Id = computerId,
                UserAssignments = new List<ComputerUserAssignment>()
            };

            var user = new User { Id = userId };
            var inUseStatus = new ComputerStatus { Id = 1, LocalizedName = "in_use" };

            _computerRepositoryMock
                .Setup(x => x.GetWithCurrentAssignmentAsync(computerId))
                .ReturnsAsync(computer);

            _userRepositoryMock
                .Setup(x => x.GetByIdAsync(userId))
                .ReturnsAsync(user);

            _statusRepositoryMock
                .Setup(x => x.GetByNameAsync("in_use"))
                .ReturnsAsync(inUseStatus);

            // Act
            var result = await _assignmentService.AssignComputerAsync(new ComputerUserAssignment
            {
                ComputerId = computerId,
                UserId = userId
            });

            // Assert
            Assert.NotNull(result);
            Assert.Equal(computerId, result.ComputerId);
            Assert.Equal(userId, result.UserId);
            Assert.Null(result.AssignEndDate);
        }

        [Fact]
        public async Task AssignComputer_ToAlreadyAssignedComputer_ShouldThrowException()
        {
            // Arrange
            var computerId = 1;
            var userId = 1;
            var computer = new Computer
            {
                Id = computerId,
                UserAssignments = new List<ComputerUserAssignment>
                {
                    new ComputerUserAssignment
                    {
                        ComputerId = computerId,
                        UserId = 2,
                        AssignStartDate = DateTime.UtcNow.AddDays(-1)
                    }
                }
            };

            _computerRepositoryMock
                .Setup(x => x.GetWithCurrentAssignmentAsync(computerId))
                .ReturnsAsync(computer);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _assignmentService.AssignComputerAsync(new ComputerUserAssignment
                {
                    ComputerId = computerId,
                    UserId = userId
                }));
        }

        [Fact]
        public async Task EndAssignment_ForAssignedComputer_ShouldSucceed()
        {
            // Arrange
            var computerId = 1;
            var userId = 1;
            var computer = new Computer
            {
                Id = computerId,
                UserAssignments = new List<ComputerUserAssignment>
                {
                    new ComputerUserAssignment
                    {
                        ComputerId = computerId,
                        UserId = userId,
                        AssignStartDate = DateTime.UtcNow.AddDays(-1)
                    }
                },
                StatusAssignments = new List<ComputerStatusAssignment>()
            };

            var availableStatus = new ComputerStatus { Id = 2, LocalizedName = "available" };

            _computerRepositoryMock
                .Setup(x => x.GetWithCurrentAssignmentAsync(computerId))
                .ReturnsAsync(computer);

            _statusRepositoryMock
                .Setup(x => x.GetByNameAsync("available"))
                .ReturnsAsync(availableStatus);

            // Act
            var result = await _assignmentService.EndAssignmentAsync(computerId, DateTime.UtcNow);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.AssignEndDate);
            Assert.Equal(computerId, result.ComputerId);
            Assert.Equal(userId, result.UserId);
        }
    }
}
