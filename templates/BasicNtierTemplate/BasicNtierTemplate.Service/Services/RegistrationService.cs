using BasicNtierTemplate.Data.Model.Identity;
using BasicNtierTemplate.Service.Contracts;
using BasicNtierTemplate.Service.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BasicNtierTemplate.Service.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly ILogger<RegistrationService> _logger;

        public RegistrationService(
            UserManager<ApplicationUser> userManager,
            IEmailService emailService,
            ILogger<RegistrationService> logger)
        {
            _userManager = userManager;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<RegistrationResult> RegisterAsync(RegisterRequest request)
        {

            var user = new ApplicationUser
            {
                UserName = request.UserName,
                Email = request.Email,
                City = request.City,
                PhoneNumber = request.PhoneNumber,
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return new RegistrationResult
                {
                    Success = false,
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }

            // Optionally send confirmation email automatically
            // TODO await SendConfirmationEmailAsync(user.Id);

            return new RegistrationResult
            {
                Success = true,
                UserId = user.Id
            };
        }

        public async Task<bool> SendConfirmationEmailAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return false;

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            // Construct confirmation link - Example: https://yourapp.com/confirm-email?token=yourToken&userId=userId
            var confirmationLink = $"https://yourapp.com/confirm-email?token={Uri.EscapeDataString(token)}&userId={user.Id}";

            var emailSent = await _emailService.SendEmailAsync(user.Email, "Confirm your email", $"Please confirm your email by clicking this link: {confirmationLink}");
            return emailSent;
        }

        public async Task<bool> ConfirmEmailAsync(string token)
        {
            // Extract userId from token or pass userId as separate parameter in real implementation
            // Here assuming token format or mapping handled elsewhere
            // You might need to adapt this method
            try
            {
                var decodedToken = Uri.UnescapeDataString(token);
                var userId = ExtractUserIdFromToken(decodedToken); // Implement this helper
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null) return false;

                var result = await _userManager.ConfirmEmailAsync(user, decodedToken);
                return result.Succeeded;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error confirming email");
                return false;
            }
        }

        public async Task<bool> IsEmailAvailableAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user == null;
        }

        // Optional helper to extract userId from token if needed
        private Guid ExtractUserIdFromToken(string token)
        {
            // Implement based on how you encode/decode tokens
            throw new NotImplementedException();
        }
    }
}
