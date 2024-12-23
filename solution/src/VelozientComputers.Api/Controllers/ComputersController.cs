using Microsoft.AspNetCore.Mvc;
using VelozientComputers.Api.DTOs;
using VelozientComputers.Core.Services;

namespace VelozientComputers.Api.Controllers
{
    /// <summary>
    /// Controller for managing computer operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ComputersController : ControllerBase
    {
        #region Fields

        private readonly IComputerService _computerService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the ComputersController
        /// </summary>
        /// <param name="computerService">Computer service instance</param>
        public ComputersController(IComputerService computerService)
        {
            _computerService = computerService;
        }

        #endregion

        #region GET Methods

        /// <summary>
        /// Gets all computers
        /// </summary>
        /// <returns>Collection of computers</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ComputerListDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ComputerListDto>>> GetComputers()
        {
            var computers = await _computerService.GetAllComputersAsync();
            return Ok(computers);
        }

        /// <summary>
        /// Gets a specific computer by id
        /// </summary>
        /// <param name="id">Computer identifier</param>
        /// <returns>Computer details</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ComputerListDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ComputerListDto>> GetComputer(int id)
        {
            var computer = await _computerService.GetComputerByIdAsync(id);
            if (computer == null)
            {
                return NotFound();
            }

            return Ok(computer);
        }

        /// <summary>
        /// Gets computers with expiring warranty
        /// </summary>
        /// <param name="daysThreshold">Days threshold for warranty expiration</param>
        /// <returns>Collection of computers with expiring warranty</returns>
        [HttpGet("expiring-warranty")]
        [ProducesResponseType(typeof(IEnumerable<ComputerListDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ComputerListDto>>> GetComputersWithExpiringWarranty([FromQuery] int daysThreshold = 30)
        {
            var computers = await _computerService.GetComputersWithExpiringWarrantyAsync(daysThreshold);
            return Ok(computers);
        }

        #endregion

        #region POST Methods

        /// <summary>
        /// Creates a new computer
        /// </summary>
        /// <param name="computerDto">Computer creation data</param>
        /// <returns>Created computer details</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ComputerListDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ComputerListDto>> CreateComputer([FromBody] CreateComputerDto computerDto)
        {
            try
            {
                var computer = await _computerService.CreateComputerAsync(computerDto);
                return CreatedAtAction(nameof(GetComputer), new { id = computer.Id }, computer);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region PUT Methods

        /// <summary>
        /// Updates an existing computer
        /// </summary>
        /// <param name="id">Computer identifier</param>
        /// <param name="computerDto">Computer update data</param>
        /// <returns>Updated computer details</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ComputerListDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ComputerListDto>> UpdateComputer(int id, [FromBody] UpdateComputerDto computerDto)
        {
            try
            {
                var computer = await _computerService.UpdateComputerAsync(id, computerDto);
                return Ok(computer);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updates the status of a computer
        /// </summary>
        /// <param name="id">Computer identifier</param>
        /// <param name="statusDto">Status update data</param>
        /// <returns>Updated computer details</returns>
        [HttpPut("{id}/status")]
        [ProducesResponseType(typeof(ComputerListDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ComputerListDto>> UpdateComputerStatus(int id, [FromBody] UpdateComputerStatusDto statusDto)
        {
            try
            {
                var computer = await _computerService.UpdateComputerStatusAsync(id, statusDto);
                return Ok(computer);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        #endregion

        #region DELETE Methods

        /// <summary>
        /// Deletes a computer
        /// </summary>
        /// <param name="id">Computer identifier</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteComputer(int id)
        {
            try
            {
                await _computerService.DeleteComputerAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        #endregion
    }
}