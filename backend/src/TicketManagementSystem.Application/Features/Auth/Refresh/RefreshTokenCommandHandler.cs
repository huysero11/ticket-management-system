using MediatR;
using TicketManagementSystem.Application.Abstractions.Repositories;
using TicketManagementSystem.Application.Abstractions.Security;
using TicketManagementSystem.Application.Features.Auth.Refresh;
using TicketManagementSystem.Domain.Entities;

namespace TicketManagementSystem.Application.Features.Auth.Refresh;

public sealed class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResponse>
{
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public RefreshTokenCommandHandler(
        IJwtTokenService jwtTokenService,
        IRefreshTokenRepository refreshTokenRepository)
    {
        _jwtTokenService = jwtTokenService;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<RefreshTokenResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var existingRefreshToken = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken, cancellationToken);

        if (existingRefreshToken is null)
        {
            throw new UnauthorizedAccessException("Invalid refresh token.");
        }

        if (existingRefreshToken.RevokedAtUtc is not null)
        {
            throw new UnauthorizedAccessException("Refresh token has been revoked.");
        }

        if (existingRefreshToken.ExpiresAtUtc <= DateTime.UtcNow)
        {
            throw new UnauthorizedAccessException("Refresh token has expired.");
        }

        existingRefreshToken.Revoke();

        var newAccessToken = _jwtTokenService.GenerateAccessToken(
            existingRefreshToken.UserId,
            existingRefreshToken.User.Email,
            existingRefreshToken.User.FullName,
            existingRefreshToken.User.Role.ToString()
        );

        var newRefreshTokenResult = _jwtTokenService.GenerateRefreshToken();
        var newRefreshToken = new RefreshToken(
            existingRefreshToken.UserId,
            newRefreshTokenResult.Token,
            newRefreshTokenResult.ExpiresAtUtc
        );

        await _refreshTokenRepository.AddAsync(newRefreshToken, cancellationToken);
        await _refreshTokenRepository.SaveChangesAsync(cancellationToken);

        return new RefreshTokenResponse(
            newAccessToken,
            newRefreshTokenResult.Token
        );
    }
}