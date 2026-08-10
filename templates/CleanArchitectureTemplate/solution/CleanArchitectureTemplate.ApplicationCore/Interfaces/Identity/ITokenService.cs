using System.Security.Claims;

namespace CleanArchitectureTemplate.ApplicationCore.Interfaces.Identity;

public interface ITokenService
{
    string GenerateToken(string userId, string email, IList<string> roles);
    Task RevokeTokenAsync(string token);
    Task<bool> IsTokenRevokedAsync(string jti);
    Task InvalidateAllRefreshTokensAsync(string userId);
    Task<bool> IsUserRevokedAsync(string userId, long tokenIssuedAt);

    #region Refresh Toker

    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    string GenerateRefreshToken();
    Task StoreRefreshTokenAsync(string userId, string refreshToken);
    Task<bool> ValidateRefreshTokenAsync(string userId, string refreshToken);
    Task RevokeRefreshTokenAsync(string userId, string refreshToken);

    #endregion
}
