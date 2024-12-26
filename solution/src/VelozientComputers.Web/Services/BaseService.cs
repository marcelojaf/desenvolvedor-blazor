using System.Net.Http.Json;
using System.Text.Json;
using VelozientComputers.Web.Models;

namespace VelozientComputers.Web.Services
{
    /// <summary>
    /// Base implementation for HTTP service operations
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TCreateDTO">DTO type for creation</typeparam>
    /// <typeparam name="TUpdateDTO">DTO type for updates</typeparam>
    public abstract class BaseService<TEntity, TCreateDTO, TUpdateDTO>
    {
        private readonly HttpClient _httpClient;
        protected readonly string _baseUrl;
        private readonly JsonSerializerOptions _jsonOptions;

        /// <summary>
        /// Initializes a new instance of the BaseService class
        /// </summary>
        /// <param name="httpClient">The HTTP client instance</param>
        /// <param name="baseUrl">Base URL for the API endpoint</param>
        protected BaseService(HttpClient httpClient, string baseUrl)
        {
            _httpClient = httpClient;
            _baseUrl = baseUrl;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        /// <summary>
        /// Gets all entities
        /// </summary>
        /// <returns>Collection of entities</returns>
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
                var response = await _httpClient.GetAsync(_baseUrl);
                var apiResponse = await HandleResponseAsync<IEnumerable<TEntity>>(response);
                return apiResponse.Data;
            }

        /// <summary>
        /// Gets an entity by its identifier
        /// </summary>
        /// <param name="id">Entity identifier</param>
        /// <returns>Entity if found</returns>
        public virtual async Task<TEntity> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/{id}");
            var apiResponse = await HandleResponseAsync<TEntity>(response);
            return apiResponse.Data;
        }

        /// <summary>
        /// Creates a new entity
        /// </summary>
        /// <param name="createDto">DTO with creation data</param>
        /// <returns>Created entity</returns>
        public virtual async Task<TEntity> CreateAsync(TCreateDTO createDto)
        {
            var response = await _httpClient.PostAsJsonAsync(_baseUrl, createDto, _jsonOptions);

            var json = await response.Content.ReadAsStringAsync();
            Console.WriteLine(json);

            var apiResponse = await HandleResponseAsync<TEntity>(response);
            return apiResponse.Data;
        }

        /// <summary>
        /// Updates an existing entity
        /// </summary>
        /// <param name="id">Entity identifier</param>
        /// <param name="updateDto">DTO with update data</param>
        /// <returns>Updated entity</returns>
        public virtual async Task<TEntity> UpdateAsync(int id, TUpdateDTO updateDto)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/{id}", updateDto, _jsonOptions);
            var apiResponse = await HandleResponseAsync<TEntity>(response);
            return apiResponse.Data;
        }

        /// <summary>
        /// Deletes an entity
        /// </summary>
        /// <param name="id">Entity identifier</param>
        public virtual async Task DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/{id}");
            await HandleResponseAsync<object>(response);
        }

        /// <summary>
        /// Handles the HTTP response and deserializes the API response wrapper
        /// </summary>
        /// <typeparam name="T">Type of the response data</typeparam>
        /// <param name="response">HTTP response message</param>
        /// <returns>API response with deserialized data</returns>
        protected virtual async Task<ApiResponse<T>> HandleResponseAsync<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"API request failed with status {response.StatusCode}: {content}");
            }

            try
            {
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<T>>(content, _jsonOptions);
                return apiResponse ?? throw new JsonException("Failed to deserialize API response");
            }
            catch (JsonException ex)
            {
                throw new JsonException($"Error deserializing response: {ex.Message}", ex);
            }
        }
    }
}