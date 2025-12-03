namespace BasicNtierTemplate.Service.Contracts
{
    public sealed class UpdateUserRequest
    {
        public Guid Id { get; init; }
        public string? Email { get; init; }
        public string? FullName { get; init; }
        public string? Password { get; init; }
        public bool? IsActive { get; init; }

        public UpdateUserRequest() { }

        public UpdateUserRequest(Guid id, string? email = null, string? fullName = null, string? password = null, bool? isActive = null)
        {
            Id = id;
            Email = email;
            FullName = fullName;
            Password = password;
            IsActive = isActive;
        }
    }

}