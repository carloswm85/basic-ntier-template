using BasicNtierTemplate.Service.Contracts;

namespace BasicNtierTemplate.Service.Services.Interfaces
{
    /// <summary>
    /// Use: This is a user-facing, higher-level process (business flow).
    /// Intended audience: End-users(self-register)
    /// </summary>
    public interface IRegistrationService
    {
        Task<RegistrationResult> RegisterAsync(RegisterRequest request);
        Task<bool> SendConfirmationEmailAsync(string userId);
        Task<bool> ConfirmEmailAsync(string token);
        Task<bool> IsEmailAvailableAsync(string email);
    }

}
