using VelozientComputers.Shared.DTOs;
using VelozientComputers.Web.Services.Interfaces;

namespace VelozientComputers.Web.Services
{
    /// <summary>
    /// Implementation of computer-related HTTP operations
    /// </summary>
    public class ComputerService : BaseService<ComputerDTO, CreateComputerDTO, UpdateComputerDTO>, IComputerService
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the ComputerService class
        /// </summary>
        /// <param name="httpClient">The HTTP client instance</param>
        public ComputerService(HttpClient httpClient)
            : base(httpClient, "api/v1/computers")
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Gets computers with expiring warranty
        /// </summary>
        /// <param name="daysThreshold">Days threshold for warranty expiration</param>
        /// <returns>Collection of computers with warranty expiring soon</returns>
        public async Task<IEnumerable<ComputerDTO>> GetComputersWithExpiringWarrantyAsync(int daysThreshold = 30)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/expiring-warranty?daysThreshold={daysThreshold}");
            var apiResponse = await HandleResponseAsync<IEnumerable<ComputerDTO>>(response);
            return apiResponse.Data;
        }

        /// <summary>
        /// Validates a computer's serial number
        /// </summary>
        /// <param name="manufacturer">Manufacturer name</param>
        /// <param name="serialNumber">Serial number to validate</param>
        /// <returns>True if valid, false otherwise</returns>
        public async Task<bool> ValidateSerialNumberAsync(string manufacturer, string serialNumber)
        {
            var response = await _httpClient.GetAsync(
                $"{_baseUrl}/validate-serial-number?manufacturer={Uri.EscapeDataString(manufacturer)}&serialNumber={Uri.EscapeDataString(serialNumber)}");
            var apiResponse = await HandleResponseAsync<bool>(response);
            return apiResponse.Data;
        }
    }
}