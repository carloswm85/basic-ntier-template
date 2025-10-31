namespace BasicNtierTemplate.Service.Contracts
{
    public class RegisterRequest
    {
        public string City { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;

        public RegisterRequest() { }
        public RegisterRequest(string city, string email, string fullName,
            string password, string phoneNumber, string username)
        {
            City = city;
            Email = email;
            FullName = fullName;
            Password = password;
            PhoneNumber = phoneNumber;
            Username = username;
        }
    }
}
