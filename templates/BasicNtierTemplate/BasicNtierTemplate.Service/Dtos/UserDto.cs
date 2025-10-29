namespace BasicNtierTemplate.Service.Dtos
{
    public class UserDto
    {
        public Guid Id { get; init; }
        public string Email { get; init; }
        public string FullName { get; init; }
        public bool IsActive { get; init; }

        public UserDto(Guid id, string email, string fullName, bool isActive)
        {
            Id = id;
            Email = email;
            FullName = fullName;
            IsActive = isActive;
        }

        public override bool Equals(object? obj)
        {
            return obj is UserDto other &&
                   Id == other.Id &&
                   Email == other.Email &&
                   FullName == other.FullName &&
                   IsActive == other.IsActive;
        }

        public override int GetHashCode() =>
            HashCode.Combine(Id, Email, FullName, IsActive);

        public override string ToString() =>
            $"{FullName} ({Email})";
    }

}
