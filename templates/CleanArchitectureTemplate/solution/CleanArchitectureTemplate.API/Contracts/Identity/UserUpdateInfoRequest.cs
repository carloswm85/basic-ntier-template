namespace CleanArchitectureTemplate.API.Contracts.Identity;

public class UserUpdateInfoRequest
{
    public string? PhoneNumber { get; set; }
    public string? CurrentPassword { get; set; }
    public string? NewPassword { get; set; }
}
