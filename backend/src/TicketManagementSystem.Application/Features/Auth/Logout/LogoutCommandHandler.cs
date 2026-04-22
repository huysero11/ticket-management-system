using MediatR;
using TicketManagementSystem.Application.Abstractions.Repositories;

namespace TicketManagementSystem.Application.Features.Auth.Logout;

public sealed class LogoutCommandHandler : IRequestHandler<LogoutCommand, Unit>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public LogoutCommandHandler(IRefreshTokenRepository refreshTokenRepository)
    {
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<Unit> Handle(LogoutCommand command, CancellationToken cancellationToken)
    {
        var refreshToken = await _refreshTokenRepository.GetByTokenAsync(
                command.RefreshToken, cancellationToken);
        if (refreshToken is null)
        {
            return Unit.Value;
        }

        if (refreshToken.RevokedAtUtc is null)
        {
            refreshToken.Revoke();
            await _refreshTokenRepository.SaveChangesAsync(cancellationToken);
        }

        return Unit.Value;
    }
}