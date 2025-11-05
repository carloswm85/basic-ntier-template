namespace BasicNtierTemplate.Service.Contracts.Authorization
{
    public class AuthorizationResult
    {
        public bool Succeeded { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public string? TokenType { get; set; } = "Bearer";
        public UserInfo? User { get; set; }
        public bool RequiresTwoFactor { get; set; }
        public bool IsLockedOut { get; set; }
        public bool IsNotAllowed { get; set; }
        public IEnumerable<string> Errors { get; set; } = [];
    }
}
