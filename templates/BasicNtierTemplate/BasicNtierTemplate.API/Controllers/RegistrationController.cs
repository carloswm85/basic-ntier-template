using BasicNtierTemplate.Service.Contracts;
using BasicNtierTemplate.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BasicNtierTemplate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;

        public RegistrationController(IRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        // POST: api/registration/register
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<RegistrationResult>> Register(string city, string email,
            string password, string phoneNumber, string username, string firstName, string lastName)
        {
            if (string.IsNullOrEmpty(city) ||
                string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(phoneNumber) ||
                string.IsNullOrEmpty(username) ||
                string.IsNullOrEmpty(firstName) ||
                string.IsNullOrEmpty(lastName))
            {
                return BadRequest("Request cannot be null.");
            }

            // Called from RegistrationController when a new user signs up
            var result = await _registrationService.RegisterAsync(new RegisterRequest
            {
                City = city,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                Password = password,
                PhoneNumber = phoneNumber,
                UserName = username
            });

            if (!result.IsSuccess)
                return BadRequest(result.Errors);

            return Ok(result);
        }

        // POST: api/registration/register/body
        [HttpPost("register/body")]
        [AllowAnonymous]
        public async Task<ActionResult<RegistrationResult>> Register([FromBody] RegisterRequest request)
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
