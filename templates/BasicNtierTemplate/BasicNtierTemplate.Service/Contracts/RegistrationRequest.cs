namespace BasicNtierTemplate.Service.Contracts
{
    public class RegisterRequest
    {
        public required string City { get; set; }
        public required string Email { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Password { get; set; }
        public required string PhoneNumber { get; set; }
        public required string UserName { get; set; }

        public RegisterRequest() { }
        public RegisterRequest(string city, string email, string firstName,
            string lastName, string password, string phoneNumber, string userName)
        {
            City = city;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            Password = password;
            PhoneNumber = phoneNumber;
            UserName = userName;
        }
    }
}
