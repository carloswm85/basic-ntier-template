namespace BasicNtierTemplate.Service.Contracts
{
    public sealed class CreateUserResponse
    {
        public required Guid UserId { get; init; }
        public required string UserName { get; init; }
        public required string Email { get; init; }
    }
}
