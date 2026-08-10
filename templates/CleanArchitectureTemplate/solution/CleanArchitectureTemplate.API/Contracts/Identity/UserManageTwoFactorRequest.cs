namespace CleanArchitectureTemplate.API.Contracts.Identity;

public class UserManageTwoFactorRequest
{
    public TwoFactorAction TwoFactorAction { get; set; }
    public string? VerificationToken2fa { get; set; }
}

public enum TwoFactorAction
{
    GenerateSetup = 1,
    VerifyAndEnable = 2,
    Disable = 3,
}
