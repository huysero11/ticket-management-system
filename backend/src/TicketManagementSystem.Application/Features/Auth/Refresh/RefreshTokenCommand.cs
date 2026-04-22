using System.ComponentModel.DataAnnotations;
using MediatR;

namespace TicketManagementSystem.Application.Features.Auth.Refresh;

public sealed record RefreshTokenCommand : IRequest<RefreshTokenResponse>
{
    [Required]
    public string RefreshToken { get; init;} = string.Empty;
}