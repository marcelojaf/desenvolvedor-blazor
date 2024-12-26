using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using VelozientComputers.Api.Controllers;
using VelozientComputers.Core.Entities;
using VelozientComputers.Core.Interfaces.Repository;
using VelozientComputers.Core.Interfaces.Service;
using VelozientComputers.Shared.DTOs;

/// <summary>
/// Controller for managing computer operations
/// </summary>
public class ComputersController : BaseController
{
    private readonly IComputerService _computerService;
    private readonly IMapper _mapper;
    private readonly IManufacturerRepository _manufacturerRepository;
    private readonly IStatusRepository _statusRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="ComputersController"/> class
    /// </summary>
    /// <param name="computerService">The computer service</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="manufacturerRepository">The manufacturer repository</param>
    /// <param name="statusRepository">The status repository</param>
    public ComputersController(IComputerService computerService, IMapper mapper, IManufacturerRepository manufacturerRepository, IStatusRepository statusRepository)
    {
        _computerService = computerService;
        _mapper = mapper;
        _manufacturerRepository = manufacturerRepository;
        _statusRepository = statusRepository;
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
            return ApiResponse(HttpStatusCode.NotFound, $"Computer with id {id} not found");

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
            var manufacturer = await _manufacturerRepository.GetByNameAsync(computerDto.Manufacturer);
            if (manufacturer == null)
                return ApiResponse(HttpStatusCode.BadRequest, $"Manufacturer '{computerDto.Manufacturer}' not found");

            var status = await _statusRepository.GetByNameAsync(computerDto.Status);
            if (status == null)
                return ApiResponse(HttpStatusCode.BadRequest, $"Status '{computerDto.Status}' not found");

            var computer = _mapper.Map<Computer>(computerDto);
            computer.ComputerManufacturerId = manufacturer.Id;

            // Criar o status assignment
            var statusAssignment = new ComputerStatusAssignment
            {
                ComputerStatusId = status.Id,
                AssignDate = DateTime.UtcNow
            };
            computer.StatusAssignments = new List<ComputerStatusAssignment> { statusAssignment };

            var createdComputer = await _computerService.CreateComputerAsync(computer);
            var createdComputerDto = _mapper.Map<ComputerDTO>(createdComputer);
            return ApiResponse(createdComputerDto, HttpStatusCode.Created);
        }
        catch (ArgumentException ex)
        {
            return ApiResponse(HttpStatusCode.BadRequest, ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return ApiResponse(HttpStatusCode.BadRequest, ex.Message);
        }
        catch (Exception ex)
        {
            return ApiResponse(HttpStatusCode.InternalServerError, ex.Message);
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
            var status = await _statusRepository.GetByNameAsync(computerDto.Status);
            if (status == null)
                return ApiResponse(HttpStatusCode.BadRequest, $"Status '{computerDto.Status}' not found");

            var computer = _mapper.Map<Computer>(computerDto);

            // Criar o status assignment para o novo status
            var statusAssignment = new ComputerStatusAssignment
            {
                ComputerStatusId = status.Id,
                AssignDate = DateTime.UtcNow
            };
            computer.StatusAssignments = new List<ComputerStatusAssignment> { statusAssignment };

            var updatedComputer = await _computerService.UpdateComputerAsync(id, computer);
            var updatedComputerDto = _mapper.Map<ComputerDTO>(updatedComputer);
            return ApiResponse(updatedComputerDto);
        }
        catch (KeyNotFoundException ex)
        {
            return ApiResponse(HttpStatusCode.NotFound, ex.Message);
        }
        catch (ArgumentException ex)
        {
            return ApiResponse(HttpStatusCode.BadRequest, ex.Message);
        }
        catch (Exception ex)
        {
            return ApiResponse(HttpStatusCode.InternalServerError, ex.Message);
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
        catch (KeyNotFoundException ex)
        {
            return ApiResponse(HttpStatusCode.NotFound, ex.Message);
        }
        catch (Exception ex)
        {
            return ApiResponse(HttpStatusCode.InternalServerError, ex.Message);
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