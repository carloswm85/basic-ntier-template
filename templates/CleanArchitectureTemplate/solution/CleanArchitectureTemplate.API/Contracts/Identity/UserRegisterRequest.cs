namespace CleanArchitectureTemplate.API.Contracts.Identity;

public class UserRegisterRequest
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string City { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}
