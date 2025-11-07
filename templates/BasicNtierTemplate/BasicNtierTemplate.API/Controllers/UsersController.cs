using BasicNtierTemplate.Service.Contracts;
using BasicNtierTemplate.Service.Dtos;
using BasicNtierTemplate.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BasicNtierTemplate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// ✅ Checks if the Users API is online and operational.
        /// </summary>
        /// <remarks>
        /// This endpoint provides a simple health check for the users service,
        /// returning the current server timestamp to confirm availability.
        /// </remarks>
        /// <response code="200">Returns confirmation that the API is online with current timestamp.</response>
        [HttpGet("status")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult Status() => Ok($"API online - {DateTime.Now}");

        /// <summary>
        /// ✅ Retrieves all registered users in the system.
        /// </summary>
        /// <remarks>
        /// This endpoint returns a collection of user objects with basic identity details such as
        /// ID, username, email, and roles. Only accessible to authorized administrators.
        /// </remarks>
        /// <response code="200">Returns the list of all users.</response>
        /// <response code="401">Unauthorized. The request requires authentication.</response>
        /// <response code="403">Forbidden. The user does not have permission to view users.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ApplicationUserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<ApplicationUserDto>>> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }


        /// <summary>
        /// 🛑 Get user by ID
        /// </summary>
        [HttpGet("{userId:guid}")]
        [ProducesResponseType(typeof(ApplicationUserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApplicationUserDto>> GetById(Guid userId)
        {
            var user = await _userService.GetByIdAsync(userId);

            if (user == null)
                return NotFound(new { message = "User not found" });

            return Ok(user);
        }

        /// <summary>
        /// 🛑 Get user by email
        /// </summary>
        [HttpGet("by-email")]
        [ProducesResponseType(typeof(ApplicationUserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApplicationUserDto>> GetByEmail([FromQuery] string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return BadRequest(new { message = "Email is required" });

            var user = await _userService.GetByEmailAsync(email);

            if (user == null)
                return NotFound(new { message = "User not found" });

            return Ok(user);
        }

        /// <summary>
        /// 🛑 Check if user exists by email
        /// </summary>
        [HttpGet("exists")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> ExistsByEmail([FromQuery] string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return BadRequest(new { message = "Email is required" });

            var exists = await _userService.ExistsByEmailAsync(email);
            return Ok(exists);
        }

        /// <summary>
        /// 🛑 Create a new user
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Create([FromBody] CreateUserRequest request)
        {
            var result = await _userService.CreateAsync(request);

            if (!result.IsSuccess)
                return BadRequest(result);

            // Assuming result.Data contains the created user ID
            return CreatedAtAction(
                nameof(GetById),
                new { userId = result.Data },
                result
            );
        }

        /// <summary>
        /// 🛑 Update an existing user
        /// </summary>
        [HttpPut("{userId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update(Guid userId, [FromBody] UpdateUserRequest request)
        {
            // Ensure the userId in the route matches the request (if applicable)
            // This depends on your UpdateUserRequest structure

            var result = await _userService.UpdateAsync(request);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// 🛑 Delete a user
        /// </summary>
        [HttpDelete("{userId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(Guid userId)
        {
            var result = await _userService.DeleteAsync(userId);

            if (!result.IsSuccess)
                return BadRequest(result);

            return NoContent();
        }

        /// <summary>
        /// 🛑 Activate a user
        /// </summary>
        [HttpPatch("{userId:guid}/activate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Activate(Guid userId)
        {
            var result = await _userService.ActivateAsync(userId);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// 🛑 Deactivate a user
        /// </summary>
        [HttpPatch("{userId:guid}/deactivate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Deactivate(Guid userId)
        {
            var result = await _userService.DeactivateAsync(userId);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }
    }
}