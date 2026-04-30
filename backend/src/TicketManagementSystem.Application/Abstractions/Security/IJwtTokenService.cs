namespace TicketManagementSystem.Application.Abstractions.Security;

public interface IJwtTokenService
{
    string GenerateAccessToken(Guid userId, string email, string fullName, string role);
    RefreshTokenResult GenerateRefreshToken();
}

public sealed record RefreshTokenResult(
    string Token,
    DateTime ExpiresAtUtc
);