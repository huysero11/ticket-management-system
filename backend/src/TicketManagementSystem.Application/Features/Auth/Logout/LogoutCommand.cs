using System.ComponentModel.DataAnnotations;
using MediatR;

namespace TicketManagementSystem.Application.Features.Auth.Logout;

public sealed record LogoutCommand : IRequest<Unit>
{
    [Required]
    public string RefreshToken { get; init; } = string.Empty;
}