using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using VelozientComputers.Core.Entities;
using VelozientComputers.Core.Interfaces.Service;
using VelozientComputers.Shared.DTOs;

namespace VelozientComputers.Api.Controllers
{
    /// <summary>
    /// Controller for managing computer operations
    /// </summary>
    public class ComputersController : BaseController
    {
        private readonly IComputerService _computerService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComputersController"/> class
        /// </summary>
        /// <param name="computerService">The computer service</param>
        /// <param name="mapper">The AutoMapper instance</param>
        public ComputersController(IComputerService computerService, IMapper mapper)
        {
            _computerService = computerService;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets all computers
        /// </summary>
        /// <returns>List of computers</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ComputerDTO>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            var computers = await _computerService.GetAllComputersAsync();
            var computersDto = _mapper.Map<IEnumerable<ComputerDTO>>(computers);
            return ApiResponse(computersDto);
        }

        /// <summary>
        /// Gets a computer by its ID
        /// </summary>
        /// <param name="id">The computer ID</param>
        /// <returns>The computer if found</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<ComputerDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var computer = await _computerService.GetComputerByIdAsync(id);
            if (computer == null)
                return ApiResponse(HttpStatusCode.NotFound);

            var computerDto = _mapper.Map<ComputerDTO>(computer);
            return ApiResponse(computerDto);
        }

        /// <summary>
        /// Gets computers with expiring warranty
        /// </summary>
        /// <param name="daysThreshold">Number of days threshold for warranty expiration</param>
        /// <returns>List of computers with warranty expiring within the threshold</returns>
        [HttpGet("expiring-warranty")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ComputerDTO>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetComputersWithExpiringWarranty([FromQuery] int daysThreshold = 30)
        {
            var computers = await _computerService.GetComputersWithExpiringWarrantyAsync(daysThreshold);
            var computersDto = _mapper.Map<IEnumerable<ComputerDTO>>(computers);
            return ApiResponse(computersDto);
        }

        /// <summary>
        /// Creates a new computer
        /// </summary>
        /// <param name="computerDto">The computer data</param>
        /// <returns>The created computer</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<ComputerDTO>), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateComputerDTO computerDto)
        {
            try
            {
                var computer = _mapper.Map<Computer>(computerDto);
                var createdComputer = await _computerService.CreateComputerAsync(computer);
                var createdComputerDto = _mapper.Map<ComputerDTO>(createdComputer);
                return ApiResponse(createdComputerDto, HttpStatusCode.Created);
            }
            catch (ArgumentException ex)
            {
                return ApiResponse(HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// Updates an existing computer
        /// </summary>
        /// <param name="id">The computer ID</param>
        /// <param name="computerDto">The updated computer data</param>
        /// <returns>The updated computer</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<ComputerDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateComputerDTO computerDto)
        {
            try
            {
                var computer = _mapper.Map<Computer>(computerDto);
                var updatedComputer = await _computerService.UpdateComputerAsync(id, computer);
                var updatedComputerDto = _mapper.Map<ComputerDTO>(updatedComputer);
                return ApiResponse(updatedComputerDto);
            }
            catch (KeyNotFoundException)
            {
                return ApiResponse(HttpStatusCode.NotFound);
            }
            catch (ArgumentException)
            {
                return ApiResponse(HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// Deletes a computer
        /// </summary>
        /// <param name="id">The computer ID</param>
        /// <returns>No content if successful</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _computerService.DeleteComputerAsync(id);
                return ApiResponse(HttpStatusCode.NoContent);
            }
            catch (KeyNotFoundException)
            {
                return ApiResponse(HttpStatusCode.NotFound);
            }
        }

        /// <summary>
        /// Validates a computer's serial number
        /// </summary>
        /// <param name="manufacturer">The manufacturer name</param>
        /// <param name="serialNumber">The serial number to validate</param>
        /// <returns>True if valid, false otherwise</returns>
        [HttpGet("validate-serial-number")]
        [ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ValidateSerialNumber([FromQuery] string manufacturer, [FromQuery] string serialNumber)
        {
            var isValid = await _computerService.ValidateSerialNumberAsync(manufacturer, serialNumber);
            return ApiResponse(isValid);
        }
    }
}