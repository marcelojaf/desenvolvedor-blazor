using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using VelozientComputers.Core.Entities;
using VelozientComputers.Core.Interfaces.Service;
using VelozientComputers.Shared.DTOs;

namespace VelozientComputers.Api.Controllers
{
    /// <summary>
    /// Controller for managing user operations
    /// </summary>
    public class UsersController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class
        /// </summary>
        /// <param name="userService">The user service</param>
        /// <param name="mapper">The AutoMapper instance</param>
        public UsersController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets all users
        /// </summary>
        /// <returns>List of users</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<UserDTO>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllUsersAsync();
            var usersDto = _mapper.Map<IEnumerable<UserDTO>>(users);
            return ApiResponse(usersDto);
        }

        /// <summary>
        /// Gets a user by their ID
        /// </summary>
        /// <param name="id">The user ID</param>
        /// <returns>The user if found</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<UserDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return ApiResponse(HttpStatusCode.NotFound);

            var userDto = _mapper.Map<UserDTO>(user);
            return ApiResponse(userDto);
        }

        /// <summary>
        /// Gets a user by their email address
        /// </summary>
        /// <param name="email">The user's email address</param>
        /// <returns>The user if found</returns>
        [HttpGet("by-email")]
        [ProducesResponseType(typeof(ApiResponse<UserDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetByEmail([FromQuery] string email)
        {
            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null)
                return ApiResponse(HttpStatusCode.NotFound);

            var userDto = _mapper.Map<UserDTO>(user);
            return ApiResponse(userDto);
        }

        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="userDto">The user data</param>
        /// <returns>The created user</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<UserDTO>), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateUserDTO userDto)
        {
            try
            {
                var user = _mapper.Map<User>(userDto);
                var createdUser = await _userService.CreateUserAsync(user);
                var createdUserDto = _mapper.Map<UserDTO>(createdUser);
                return ApiResponse(createdUserDto, HttpStatusCode.Created);
            }
            catch (InvalidOperationException)
            {
                return ApiResponse(HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// Updates an existing user
        /// </summary>
        /// <param name="id">The user ID</param>
        /// <param name="userDto">The updated user data</param>
        /// <returns>The updated user</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<UserDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDTO userDto)
        {
            try
            {
                var user = _mapper.Map<User>(userDto);
                var updatedUser = await _userService.UpdateUserAsync(id, user);
                var updatedUserDto = _mapper.Map<UserDTO>(updatedUser);
                return ApiResponse(updatedUserDto);
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
        /// Deletes a user
        /// </summary>
        /// <param name="id">The user ID</param>
        /// <returns>No content if successful</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _userService.DeleteUserAsync(id);
                return ApiResponse(HttpStatusCode.NoContent);
            }
            catch (KeyNotFoundException)
            {
                return ApiResponse(HttpStatusCode.NotFound);
            }
        }
    }
}