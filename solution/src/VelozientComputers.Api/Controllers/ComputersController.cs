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
    public ComputersController(IComputerService computerService,
        IMapper mapper,
        IManufacturerRepository manufacturerRepository,
        IStatusRepository statusRepository)
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
        return ApiResponse(_mapper.Map<IEnumerable<ComputerDTO>>(computers));
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

        return ApiResponse(_mapper.Map<ComputerDTO>(computer));
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
        return ApiResponse(_mapper.Map<IEnumerable<ComputerDTO>>(computers));
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
            var (manufacturer, status) = await ValidateManufacturerAndStatus(computerDto.Manufacturer, computerDto.Status);
            var computer = CreateComputerEntity(computerDto, manufacturer, status);

            var createdComputer = await _computerService.CreateComputerAsync(computer);
            return ApiResponse(_mapper.Map<ComputerDTO>(createdComputer), HttpStatusCode.Created);
        }
        catch (ArgumentException ex)
        {
            return ApiResponse(HttpStatusCode.BadRequest, ex.Message);
        }
        catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
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
            var existingComputer = await _computerService.GetComputerByIdAsync(id);
            if (existingComputer == null)
                return ApiResponse(HttpStatusCode.NotFound, $"Computer with id {id} not found");

            var (manufacturer, status) = await ValidateManufacturerAndStatus(computerDto.Manufacturer, computerDto.Status);
            var computer = UpdateComputerEntity(computerDto, manufacturer, status, existingComputer);

            var updatedComputer = await _computerService.UpdateComputerAsync(id, computer);
            return ApiResponse(_mapper.Map<ComputerDTO>(updatedComputer));
        }
        catch (ArgumentException ex)
        {
            return ApiResponse(HttpStatusCode.BadRequest, ex.Message);
        }
        catch (Exception ex) when (ex is KeyNotFoundException || ex is ArgumentException)
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

    #region Private Methods

    /// <summary>
    /// Validates manufacturer and status existence
    /// </summary>
    /// <param name="manufacturerName">Name of the manufacturer</param>
    /// <param name="statusName">Name of the status</param>
    /// <returns>Tuple containing the manufacturer and status entities</returns>
    private async Task<(ComputerManufacturer Manufacturer, ComputerStatus Status)> ValidateManufacturerAndStatus(
        string manufacturerName, string statusName)
    {
        var manufacturer = await _manufacturerRepository.GetByNameAsync(manufacturerName);
        if (manufacturer == null)
            throw new ArgumentException($"Manufacturer '{manufacturerName}' not found");

        var status = await _statusRepository.GetByNameAsync(statusName);
        if (status == null)
            throw new ArgumentException($"Status '{statusName}' not found");

        return (manufacturer, status);
    }

    /// <summary>
    /// Creates a new computer entity with status assignment
    /// </summary>
    /// <param name="computerDto">The computer DTO</param>
    /// <param name="manufacturer">The manufacturer entity</param>
    /// <param name="status">The status entity</param>
    /// <returns>Computer entity ready for creation</returns>
    private Computer CreateComputerEntity(CreateComputerDTO computerDto,
        ComputerManufacturer manufacturer, ComputerStatus status)
    {
        var computer = _mapper.Map<Computer>(computerDto);
        computer.ComputerManufacturerId = manufacturer.Id;
        computer.StatusAssignments = new List<ComputerStatusAssignment>
        {
            new ComputerStatusAssignment
            {
                ComputerStatusId = status.Id,
                AssignDate = DateTime.UtcNow
            }
        };

        return computer;
    }

    /// <summary>
    /// Updates a computer entity with new status assignment if needed
    /// </summary>
    /// <param name="computerDto">The update computer DTO</param>
    /// <param name="manufacturer">The manufacturer entity</param>
    /// <param name="status">The status entity</param>
    /// <param name="existingComputer">The existing computer entity</param>
    /// <returns>Computer entity ready for update</returns>
    private Computer UpdateComputerEntity(UpdateComputerDTO computerDto,
        ComputerManufacturer manufacturer, ComputerStatus status, Computer existingComputer)
    {
        var computer = _mapper.Map<Computer>(computerDto, opt => opt.Items["Manufacturer"] = manufacturer);

        var currentStatus = existingComputer.StatusAssignments
            .OrderByDescending(sa => sa.AssignDate)
            .FirstOrDefault()?.Status;

        if (currentStatus == null || currentStatus.Id != status.Id)
        {
            computer.StatusAssignments = new List<ComputerStatusAssignment>
            {
                new ComputerStatusAssignment
                {
                    ComputerStatusId = status.Id,
                    AssignDate = DateTime.UtcNow
                }
            };
        }

        return computer;
    }

    #endregion
}