namespace CleanArchitectureTemplate.API.Contracts.Identity;

public class ManageTwoFactorResponse
{
    public string? SharedKey { get; set; } // Secret key used to configure the authenticator app (Base32 string)
    public string? AuthenticatorUri { get; set; } // URI used to generate a QR code for apps like Google Authenticator.
    public string Message { get; set; } = default!; // Success/error/status message (non-nullable)
    public IEnumerable<string>? RecoveryCodes { get; set; }
}
