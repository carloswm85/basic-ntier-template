using BasicNtierTemplate.Service.Contracts;
using BasicNtierTemplate.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BasicNtierTemplate.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegistrationsController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;

        public RegistrationsController(IRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        /// <summary>
        /// ✅ Checks if the registration API is online and operational.
        /// </summary>
        /// <remarks>
        /// This endpoint provides a simple health check for the registration service,
        /// returning the current server timestamp to confirm availability.
        /// </remarks>
        /// <response code="200">Returns confirmation that the API is online with current timestamp.</response>
        [HttpGet("status")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult Status() => Ok($"API online - {DateTime.Now}");

        /// <summary>
        /// 🛑 Registers a new user account
        /// </summary>
        /// <remarks>
        /// This endpoint creates a new user account in the system. It validates the registration
        /// request and returns the registration result with user information upon successful creation.
        /// </remarks>
        /// <param name="request">User registration details</param>
        /// <returns>Registration result with user information</returns>
        /// <response code="200">Returns the registration result with newly created user information.</response>
        /// <response code="400">Bad request. The request is null or registration failed with validation errors.</response>
        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(RegistrationResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RegistrationResult>> RegisterAsync([FromBody] RegisterRequest request)
        {
            if (request == null)
                return BadRequest("Request cannot be null.");

            var result = await _registrationService.RegisterAsync(request);

            if (!result.IsSuccess)
                return BadRequest(result.Errors);

            return Ok(result);
        }

        /// <summary>
        /// 🛑 Sends a confirmation email to a registered user.
        /// </summary>
        /// <remarks>
        /// This endpoint triggers the sending of an email confirmation link to the specified user.
        /// The user must verify their email address by clicking the link in the confirmation email.
        /// </remarks>
        /// <param name="userId">The unique identifier of the user</param>
        /// <returns>Result indicating whether the email was sent successfully</returns>
        /// <response code="200">Confirmation email sent successfully.</response>
        /// <response code="400">Bad request. Failed to send confirmation email.</response>
        [HttpPost("send-confirmation-email/{userId}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> SendConfirmationEmail(Guid userId)
        {
            var result = await _registrationService.SendConfirmationEmailAsync(userId.ToString());

            if (!result)
                return BadRequest("Failed to send confirmation email.");

            return Ok("Confirmation email sent successfully.");
        }

        /// <summary>
        /// 🛑 Confirms a user's email address using a verification token.
        /// </summary>
        /// <remarks>
        /// This endpoint validates the email confirmation token sent to the user and activates
        /// their account upon successful verification. Users typically access this endpoint by
        /// clicking the confirmation link in their email.
        /// </remarks>
        /// <param name="token">The email confirmation token</param>
        /// <param name="userId">The unique identifier of the user</param>
        /// <returns>Result indicating whether the email was confirmed successfully</returns>
        /// <response code="200">Email confirmed successfully.</response>
        /// <response code="400">Bad request. Token is required or email confirmation failed.</response>
        [HttpGet("confirm-email")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> ConfirmEmail([FromQuery] string token, [FromQuery] Guid userId)
        {
            if (string.IsNullOrEmpty(token))
                return BadRequest("Token is required.");

            var result = await _registrationService.ConfirmEmailAsync(token);

            if (!result)
                return BadRequest("Email confirmation failed.");

            return Ok("Email confirmed successfully.");
        }

        /// <summary>
        /// 🛑 Checks if an email address is available for registration.
        /// </summary>
        /// <remarks>
        /// This endpoint verifies whether the provided email address is already registered in the system.
        /// Useful for client-side validation during the registration process to provide immediate feedback.
        /// </remarks>
        /// <param name="email">The email address to check</param>
        /// <returns>Boolean indicating whether the email is available</returns>
        /// <response code="200">Returns true if the email is available, false if already registered.</response>
        /// <response code="400">Bad request. Email parameter is required.</response>
        [HttpGet("is-email-available")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> IsEmailAvailable([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest("Email is required.");

            var isAvailable = await _registrationService.IsEmailAvailableAsync(email);
            return Ok(isAvailable);
        }
    }
}