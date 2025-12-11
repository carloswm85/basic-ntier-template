namespace BasicNtierTemplate.Service.Dtos.User
{
    public class ApplicationUserDto
    {
        public ApplicationUserDto() { }

        public Guid Id { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string City { get; set; } = default!;

        public string Email { get; set; } = default!;
        public bool EmailConfirmed { get; set; }
        public bool IsActive { get; set; }

        public string? PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }

        // Optional, depending on your app
        public IList<string> Roles { get; set; } = new List<string>();
        public IList<string> Claims { get; set; } = new List<string>();


    }

}
