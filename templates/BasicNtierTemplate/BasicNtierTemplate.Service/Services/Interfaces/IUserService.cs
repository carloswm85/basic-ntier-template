
namespace BasicNtierTemplate.Service.Services.Interfaces
{
    public interface IUserService
    {
        /* EXAMPLES AI GENERATED
        #region Authentication

        /// <summary>
        /// Registers a new user with the provided credentials
        /// </summary>
        /// <param name="registration">Registration request containing email and password</param>
        /// <returns>Result indicating success or failure with error details</returns>
        Task<RegistrationResult> RegisterAsync(RegisterRequest registration);

        /// <summary>
        /// Authenticates a user and returns access tokens
        /// </summary>
        /// <param name="login">Login request containing credentials and optional 2FA codes</param>
        /// <param name="useCookies">Whether to use cookie-based authentication</param>
        /// <param name="useSessionCookies">Whether to use session cookies</param>
        /// <returns>Access token response or null if authentication fails</returns>
        Task<AccessTokenResponse?> LoginAsync(LoginRequest login, bool? useCookies, bool? useSessionCookies);

        /// <summary>
        /// Refreshes an access token using a refresh token
        /// </summary>
        /// <param name="refreshRequest">Refresh token request</param>
        /// <returns>New access token response or null if refresh fails</returns>
        Task<AccessTokenResponse?> RefreshTokenAsync(RefreshRequest refreshRequest);

        #endregion

        #region Email Confirmation

        /// <summary>
        /// Confirms a user's email address
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <param name="code">Confirmation code</param>
        /// <param name="changedEmail">New email if this is an email change confirmation</param>
        /// <returns>Result indicating success or failure</returns>
        Task<ConfirmationResult> ConfirmEmailAsync(string userId, string code, string changedEmail);

        /// <summary>
        /// Resends the email confirmation link to the user
        /// </summary>
        /// <param name="resendRequest">Request containing the user's email</param>
        /// <returns>Result indicating success or failure</returns>
        Task<ResendResult> ResendConfirmationEmailAsync(ResendConfirmationEmailRequest resendRequest);

        #endregion

        #region Password Reset

        /// <summary>
        /// Initiates the password reset process by sending a reset code to the user's email
        /// </summary>
        /// <param name="resetRequest">Request containing the user's email</param>
        /// <returns>Result indicating success or failure</returns>
        Task<ForgotPasswordResult> ForgotPasswordAsync(ForgotPasswordRequest resetRequest);

        /// <summary>
        /// Resets the user's password using the provided reset code
        /// </summary>
        /// <param name="resetRequest">Request containing email, reset code, and new password</param>
        /// <returns>Result indicating success or failure</returns>
        Task<ResetPasswordResult> ResetPasswordAsync(ResetPasswordRequest resetRequest);

        #endregion

        #region User Management

        /// <summary>
        /// Manages two-factor authentication settings for the authenticated user
        /// </summary>
        /// <param name="tfaRequest">2FA configuration request</param>
        /// <returns>2FA response with shared key and recovery codes, or null if user not found</returns>
        Task<TwoFactorResponse?> Manage2faAsync(TwoFactorRequest tfaRequest);

        /// <summary>
        /// Gets the authenticated user's information
        /// </summary>
        /// <returns>User info response or null if user not found</returns>
        Task<InfoResponse?> GetInfoAsync();

        /// <summary>
        /// Updates the authenticated user's information
        /// </summary>
        /// <param name="infoRequest">Request containing updated email and/or password</param>
        /// <returns>Updated user info response or null if user not found</returns>
        Task<InfoResponse?> UpdateInfoAsync(InfoRequest infoRequest);

        #endregion
        */
    }
}