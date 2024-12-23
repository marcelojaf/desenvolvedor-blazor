using Microsoft.AspNetCore.Mvc;
using VelozientComputers.Api.DTO;
using VelozientComputers.Core.Services;

namespace VelozientComputers.Api.Controllers
{
    /// <summary>
    /// Controller for managing computer assignments
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AssignmentsController : ControllerBase
    {
        #region Fields

        private readonly IAssignmentService _assignmentService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the AssignmentsController
        /// </summary>
        /// <param name="assignmentService">Assignment service instance</param>
        public AssignmentsController(IAssignmentService assignmentService)
        {
            _assignmentService = assignmentService;
        }

        #endregion

        #region GET Methods

        /// <summary>
        /// Gets the current assignment for a computer
        /// </summary>
        /// <param name="computerId">Computer identifier</param>
        /// <returns>Current assignment details</returns>
        [HttpGet("computers/{computerId}/current")]
        [ProducesResponseType(typeof(ComputerAssignmentDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ComputerAssignmentDto>> GetCurrentAssignment(int computerId)
        {
            try
            {
                var assignment = await _assignmentService.GetCurrentAssignmentAsync(computerId);
                if (assignment == null)
                {
                    return NotFound("No current assignment found for this computer");
                }
                return Ok(assignment);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Computer not found");
            }
        }

        /// <summary>
        /// Gets all assignments for a computer
        /// </summary>
        /// <param name="computerId">Computer identifier</param>
        /// <returns>Collection of assignments</returns>
        [HttpGet("computers/{computerId}/history")]
        [ProducesResponseType(typeof(IEnumerable<ComputerAssignmentDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ComputerAssignmentDto>>> GetComputerAssignmentHistory(int computerId)
        {
            try
            {
                var assignments = await _assignmentService.GetComputerAssignmentHistoryAsync(computerId);
                return Ok(assignments);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Computer not found");
            }
        }

        /// <summary>
        /// Gets all assignments for a user
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>Collection of assignments</returns>
        [HttpGet("users/{userId}/history")]
        [ProducesResponseType(typeof(IEnumerable<ComputerAssignmentDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ComputerAssignmentDto>>> GetUserAssignmentHistory(int userId)
        {
            try
            {
                var assignments = await _assignmentService.GetUserAssignmentHistoryAsync(userId);
                return Ok(assignments);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("User not found");
            }
        }

        #endregion

        #region POST Methods

        /// <summary>
        /// Assigns a computer to a user
        /// </summary>
        /// <param name="assignmentDto">Assignment details</param>
        /// <returns>Created assignment details</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ComputerAssignmentDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ComputerAssignmentDto>> AssignComputer([FromBody] ComputerAssignmentDto assignmentDto)
        {
            try
            {
                var assignment = await _assignmentService.AssignComputerAsync(assignmentDto);
                return CreatedAtAction(
                    nameof(GetCurrentAssignment),
                    new { computerId = assignment.ComputerId },
                    assignment);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region PUT Methods

        /// <summary>
        /// Ends a computer assignment
        /// </summary>
        /// <param name="computerId">Computer identifier</param>
        /// <param name="endDate">End date of the assignment</param>
        /// <returns>Updated assignment details</returns>
        [HttpPut("computers/{computerId}/end-assignment")]
        [ProducesResponseType(typeof(ComputerAssignmentDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ComputerAssignmentDto>> EndAssignment(int computerId, [FromBody] DateTime endDate)
        {
            try
            {
                var assignment = await _assignmentService.EndAssignmentAsync(computerId, endDate);
                return Ok(assignment);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Computer not found");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion
    }
}