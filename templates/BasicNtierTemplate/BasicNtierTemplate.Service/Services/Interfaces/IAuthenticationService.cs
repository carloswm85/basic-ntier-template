using BasicNtierTemplate.Service.Contracts.Authorization;

public interface IAuthenticationService
{
    #region Authentication & Authorization

    Task<AuthorizationResult> LoginAsync(LoginRequest request);
    Task LogoutAsync(Guid userId);
    Task<bool> ValidateCredentialsAsync(string email, string password);

    #endregion

    //#region Token Management
    //
    //Task<AccessTokenResponse?> RefreshTokenAsync(RefreshRequest refreshRequest);
    //Task<bool> ValidateTokenAsync(string token);
    //Task<bool> RevokeTokensAsync(Guid userId);
    //Task RevokeAllTokensAsync(Guid userId);
    //
    //#endregion
    //
    //#region Email Confirmation
    //
    //Task<ConfirmationResult> ConfirmEmailAsync(string userId, string code, string changedEmail);
    //Task<ResendResult> ResendConfirmationEmailAsync(ResendConfirmationEmailRequest resendRequest);
    //Task<bool> IsEmailConfirmedAsync(Guid userId);
    //
    //#endregion
    //
    //#region Password Management
    //
    //Task<ForgotPasswordResult> ForgotPasswordAsync(ForgotPasswordRequest resetRequest);
    //Task<ResetPasswordResult> ResetPasswordAsync(ResetPasswordRequest resetRequest);
    //Task<ChangePasswordResult> ChangePasswordAsync(ChangePasswordRequest request);
    //Task<bool> ValidatePasswordStrengthAsync(string password);
    //
    //#endregion
    //
    //#region Two-Factor Authentication
    //
    //Task<TwoFactorResponse?> Manage2faAsync(TwoFactorRequest tfaRequest);
    //Task<TwoFactorResponse?> Enable2faAsync(Guid userId);
    //Task<TwoFactorResponse?> Disable2faAsync(Guid userId);
    //Task<RecoveryCodesResponse?> GenerateRecoveryCodesAsync(Guid userId);
    //Task<bool> Verify2faCodeAsync(Guid userId, string code);
    //
    //#endregion
    //
    //#region Session & Security Management
    //
    //Task<IEnumerable<SessionInfo>> GetActiveSessionsAsync(Guid userId);
    //Task<bool> RevokeSessionAsync(Guid userId, string sessionId);
    //Task<SecurityLogResponse?> GetSecurityLogAsync(Guid userId);
    //
    //#endregion
    //
    //#region Account Management
    //
    //Task<bool> LockAccountAsync(Guid userId, TimeSpan? duration);
    //Task<bool> UnlockAccountAsync(Guid userId);
    //
    //#endregion
    //
    //#region User Information
    //
    //Task<InfoResponse?> GetInfoAsync();
    //Task<InfoResponse?> UpdateInfoAsync(InfoRequest infoRequest);
    //
    //#endregion
}