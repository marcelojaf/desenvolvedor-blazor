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
    }
}