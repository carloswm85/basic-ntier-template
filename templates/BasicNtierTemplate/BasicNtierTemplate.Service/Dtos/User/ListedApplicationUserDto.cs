namespace BasicNtierTemplate.Service.Dtos.User
{
    public class ListedApplicationUserDto
    {
        public ListedApplicationUserDto() { }

        public Guid Id { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public bool EmailConfirmed { get; set; }
        public bool IsActive { get; set; }
    }

}
