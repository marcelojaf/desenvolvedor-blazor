using VelozientComputers.Shared.DTOs;
using VelozientComputers.Web.Services.Interfaces;

namespace VelozientComputers.Web.Services
{
    /// <summary>
    /// Implementation of user-related HTTP operations
    /// </summary>
    public class UserService : BaseService<UserDTO, CreateUserDTO, UpdateUserDTO>, IUserService
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the UserService class
        /// </summary>
        /// <param name="httpClient">The HTTP client instance</param>
        public UserService(HttpClient httpClient)
            : base(httpClient, "api/v1/users")
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Gets a user by their email address
        /// </summary>
        /// <param name="email">User's email address</param>
        /// <returns>User if found</returns>
        public async Task<UserDTO> GetByEmailAsync(string email)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/by-email?email={Uri.EscapeDataString(email)}");
            var apiResponse = await HandleResponseAsync<UserDTO>(response);
            return apiResponse.Data;
        }
    }
}