namespace CleanArchitectureTemplate.API.Contracts.Identity;

public class UserResetPasswordRequest
{
    public required string Email { get; set; }
    public required string Token { get; set; }
    public required string NewPassword { get; set; }
}
