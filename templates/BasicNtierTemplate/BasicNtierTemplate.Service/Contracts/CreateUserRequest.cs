namespace BasicNtierTemplate.Service.Contracts
{
    public sealed class CreateUserRequest
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }

        public CreateUserRequest(string email, string fullName, string password, string username)
        {
            Email = email;
            FullName = fullName;
            Password = password;
            UserName = username;
        }
    }

}
