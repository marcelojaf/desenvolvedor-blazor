using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using VelozientComputers.Api.DTOs;
using VelozientComputers.Core.Entities;
using VelozientComputers.Core.Interfaces.Service;

namespace VelozientComputers.Api.Controllers
{
    /// <summary>
    /// Controller for managing computer-user assignments
    /// </summary>
    public class AssignmentsController : BaseController
    {
        private readonly IAssignmentService _assignmentService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssignmentsController"/> class
        /// </summary>
        /// <param name="assignmentService">The assignment service</param>
        /// <param name="mapper">The AutoMapper instance</param>
        public AssignmentsController(IAssignmentService assignmentService, IMapper mapper)
        {
            _assignmentService = assignmentService;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets the current assignment for a computer
        /// </summary>
        /// <param name="computerId">The computer ID</param>
        /// <returns>The current assignment details if exists</returns>
        [HttpGet("computers/{computerId}/current")]
        [ProducesResponseType(typeof(ApiResponse<UserAssignmentDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetCurrentAssignment(int computerId)
        {
            try
            {
                var assignment = await _assignmentService.GetCurrentAssignmentAsync(computerId);
                if (assignment == null)
                    return ApiResponse(HttpStatusCode.NotFound);

                var assignmentDto = _mapper.Map<UserAssignmentDTO>(assignment);
                return ApiResponse(assignmentDto);
            }
            catch (KeyNotFoundException)
            {
                return ApiResponse(HttpStatusCode.NotFound);
            }
        }

        /// <summary>
        /// Gets the assignment history for a computer
        /// </summary>
        /// <param name="computerId">The computer ID</param>
        /// <returns>List of all assignments for the computer</returns>
        [HttpGet("computers/{computerId}/history")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<UserAssignmentDTO>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetComputerAssignmentHistory(int computerId)
        {
            try
            {
                var assignments = await _assignmentService.GetComputerAssignmentHistoryAsync(computerId);
                var assignmentsDto = _mapper.Map<IEnumerable<UserAssignmentDTO>>(assignments);
                return ApiResponse(assignmentsDto);
            }
            catch (KeyNotFoundException)
            {
                return ApiResponse(HttpStatusCode.NotFound);
            }
        }

        /// <summary>
        /// Gets the assignment history for a user
        /// </summary>
        /// <param name="userId">The user ID</param>
        /// <returns>List of all assignments for the user</returns>
        [HttpGet("users/{userId}/history")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ComputerAssignmentDTO>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetUserAssignmentHistory(int userId)
        {
            try
            {
                var assignments = await _assignmentService.GetUserAssignmentHistoryAsync(userId);
                var assignmentsDto = _mapper.Map<IEnumerable<ComputerAssignmentDTO>>(assignments);
                return ApiResponse(assignmentsDto);
            }
            catch (KeyNotFoundException)
            {
                return ApiResponse(HttpStatusCode.NotFound);
            }
        }

        /// <summary>
        /// Assigns a computer to a user
        /// </summary>
        /// <param name="computerId">The computer ID</param>
        /// <param name="userId">The user ID to assign the computer to</param>
        /// <returns>The new assignment details</returns>
        [HttpPost("computers/{computerId}/assign/{userId}")]
        [ProducesResponseType(typeof(ApiResponse<UserAssignmentDTO>), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AssignComputer(int computerId, int userId)
        {
            try
            {
                var assignment = new ComputerUserAssignment
                {
                    ComputerId = computerId,
                    UserId = userId,
                };

                var result = await _assignmentService.AssignComputerAsync(assignment);
                var assignmentDto = _mapper.Map<UserAssignmentDTO>(result);
                return ApiResponse(assignmentDto, HttpStatusCode.Created);
            }
            catch (KeyNotFoundException)
            {
                return ApiResponse(HttpStatusCode.NotFound);
            }
            catch (InvalidOperationException)
            {
                return ApiResponse(HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// Ends the current assignment for a computer
        /// </summary>
        /// <param name="computerId">The computer ID</param>
        /// <returns>The updated assignment details</returns>
        [HttpPost("computers/{computerId}/end-assignment")]
        [ProducesResponseType(typeof(ApiResponse<UserAssignmentDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> EndAssignment(int computerId)
        {
            try
            {
                var result = await _assignmentService.EndAssignmentAsync(computerId, DateTime.UtcNow);
                var assignmentDto = _mapper.Map<UserAssignmentDTO>(result);
                return ApiResponse(assignmentDto);
            }
            catch (KeyNotFoundException)
            {
                return ApiResponse(HttpStatusCode.NotFound);
            }
            catch (InvalidOperationException)
            {
                return ApiResponse(HttpStatusCode.BadRequest);
            }
        }
    }
}