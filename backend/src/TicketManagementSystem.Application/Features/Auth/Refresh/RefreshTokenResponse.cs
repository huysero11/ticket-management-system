namespace TicketManagementSystem.Application.Features.Auth.Refresh;

public sealed record RefreshTokenResponse(
    string AccessToken,
    string RefreshToken
);