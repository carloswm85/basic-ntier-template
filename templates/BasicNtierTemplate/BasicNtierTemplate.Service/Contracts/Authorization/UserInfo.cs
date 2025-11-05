namespace BasicNtierTemplate.Service.Contracts.Authorization
{
    public class UserInfo
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public required IEnumerable<string> Roles { get; set; } = [];
    }
}
