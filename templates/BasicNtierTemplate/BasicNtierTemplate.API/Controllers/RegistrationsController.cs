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

        [HttpGet("status")]
        public IActionResult Status() => Ok($"API online - {DateTime.Now}");

        /// <summary>
        /// Registers a new user account
        /// </summary>
        /// <param name="request">User registration details</param>
        /// <returns>Registration result with user information</returns>
        // POST: api2/apiregistration/register
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<RegistrationResult>> RegisterAsync([FromBody] RegisterRequest request)
        {
            if (request == null)
                return BadRequest("Request cannot be null.");

            var result = await _registrationService.RegisterAsync(request);

            if (!result.IsSuccess)
                return BadRequest(result.Errors);

            return Ok(result);
        }

        // POST: api/registration/send-confirmation-email/{userId}
        [HttpPost("send-confirmation-email/{userId}")]
        [AllowAnonymous]
        public async Task<ActionResult> SendConfirmationEmail(Guid userId)
        {

            var result = await _registrationService.SendConfirmationEmailAsync(userId.ToString());

            if (!result)
                return BadRequest("Failed to send confirmation email.");

            return Ok("Confirmation email sent successfully.");
        }

        // GET: api/registration/confirm-email
        [HttpGet("confirm-email")]
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail([FromQuery] string token, [FromQuery] Guid userId)
        {
            if (string.IsNullOrEmpty(token))
                return BadRequest("Token is required.");

            var result = await _registrationService.ConfirmEmailAsync(token);

            if (!result)
                return BadRequest("Email confirmation failed.");

            return Ok("Email confirmed successfully.");
        }

        // GET: api/registration/is-email-available
        [HttpGet("is-email-available")]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> IsEmailAvailable([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest("Email is required.");

            var isAvailable = await _registrationService.IsEmailAvailableAsync(email);
            return Ok(isAvailable);
        }
    }
}
