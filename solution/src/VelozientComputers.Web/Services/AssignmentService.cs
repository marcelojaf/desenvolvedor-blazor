using VelozientComputers.Shared.DTOs;
using VelozientComputers.Web.Services.Interfaces;

namespace VelozientComputers.Web.Services
{
    /// <summary>
    /// Implementation of computer assignment operations
    /// </summary>
    public class AssignmentService : BaseService<UserAssignmentDTO, object, object>, IAssignmentService
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the AssignmentService class
        /// </summary>
        /// <param name="httpClient">The HTTP client instance</param>
        public AssignmentService(HttpClient httpClient)
            : base(httpClient, "api/v1/assignments")
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Gets the current assignment for a computer
        /// </summary>
        /// <param name="computerId">Computer identifier</param>
        /// <returns>Current user assignment if exists</returns>
        public async Task<UserAssignmentDTO> GetCurrentAssignmentAsync(int computerId)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/computers/{computerId}/current");
            var apiResponse = await HandleResponseAsync<UserAssignmentDTO>(response);
            return apiResponse.Data;
        }

        /// <summary>
        /// Gets the assignment history for a computer
        /// </summary>
        /// <param name="computerId">Computer identifier</param>
        /// <returns>Collection of user assignments</returns>
        public async Task<IEnumerable<UserAssignmentDTO>> GetComputerAssignmentHistoryAsync(int computerId)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/computers/{computerId}/history");
            var apiResponse = await HandleResponseAsync<IEnumerable<UserAssignmentDTO>>(response);
            return apiResponse.Data;
        }

        /// <summary>
        /// Gets the assignment history for a user
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>Collection of computer assignments</returns>
        public async Task<IEnumerable<ComputerAssignmentDTO>> GetUserAssignmentHistoryAsync(int userId)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/users/{userId}/history");
            var apiResponse = await HandleResponseAsync<IEnumerable<ComputerAssignmentDTO>>(response);
            return apiResponse.Data;
        }

        /// <summary>
        /// Assigns a computer to a user
        /// </summary>
        /// <param name="computerId">Computer identifier</param>
        /// <param name="userId">User identifier</param>
        /// <returns>New assignment details</returns>
        public async Task<UserAssignmentDTO> AssignComputerToUserAsync(int computerId, int userId)
        {
            var response = await _httpClient.PostAsync($"{_baseUrl}/computers/{computerId}/assign/{userId}", null);
            var apiResponse = await HandleResponseAsync<UserAssignmentDTO>(response);
            return apiResponse.Data;
        }

        /// <summary>
        /// Ends the current assignment for a computer
        /// </summary>
        /// <param name="computerId">Computer identifier</param>
        /// <returns>Updated assignment details</returns>
        public async Task<UserAssignmentDTO> EndAssignmentAsync(int computerId)
        {
            var response = await _httpClient.PostAsync($"{_baseUrl}/computers/{computerId}/end-assignment", null);
            var apiResponse = await HandleResponseAsync<UserAssignmentDTO>(response);
            return apiResponse.Data;
        }
    }
}