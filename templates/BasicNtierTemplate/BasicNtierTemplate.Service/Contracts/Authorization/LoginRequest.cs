namespace BasicNtierTemplate.Service.Contracts.Authorization
{
    public class LoginRequest
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required bool RememberMe { get; set; }
        public string? TwoFactorCode { get; set; }
        public string? TwoFactorRecoveryCode { get; set; }
    }
}
