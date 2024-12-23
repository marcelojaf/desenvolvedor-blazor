using Microsoft.AspNetCore.Mvc;
using VelozientComputers.Api.DTOss;
using VelozientComputers.Core.Services;

namespace VelozientComputers.Api.Controllers
{
    /// <summary>
    /// Controller for managing user operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        #region Fields

        private readonly IUserService _userService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the UsersController
        /// </summary>
        /// <param name="userService">User service instance</param>
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        #endregion

        #region GET Methods

        /// <summary>
        /// Gets all users
        /// </summary>
        /// <returns>Collection of users</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserListDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserListDto>>> GetUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        /// <summary>
        /// Gets a specific user by id
        /// </summary>
        /// <param name="id">User identifier</param>
        /// <returns>User details</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserListDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserListDto>> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        /// <summary>
        /// Gets a user by email
        /// </summary>
        /// <param name="email">User email address</param>
        /// <returns>User details</returns>
        [HttpGet("by-email")]
        [ProducesResponseType(typeof(UserListDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserListDto>> GetUserByEmail([FromQuery] string email)
        {
            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        #endregion

        #region POST Methods

        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="userDto">User creation data</param>
        /// <returns>Created user details</returns>
        [HttpPost]
        [ProducesResponseType(typeof(UserListDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserListDto>> CreateUser([FromBody] CreateUserDto userDto)
        {
            try
            {
                var user = await _userService.CreateUserAsync(userDto);
                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region PUT Methods

        /// <summary>
        /// Updates an existing user
        /// </summary>
        /// <param name="id">User identifier</param>
        /// <param name="userDto">User update data</param>
        /// <returns>Updated user details</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UserListDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserListDto>> UpdateUser(int id, [FromBody] UpdateUserDto userDto)
        {
            try
            {
                var user = await _userService.UpdateUserAsync(id, userDto);
                return Ok(user);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region DELETE Methods

        /// <summary>
        /// Deletes a user
        /// </summary>
        /// <param name="id">User identifier</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                await _userService.DeleteUserAsync(id);
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