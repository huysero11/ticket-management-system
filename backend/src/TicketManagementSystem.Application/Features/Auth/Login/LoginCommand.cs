using MediatR;
using System.ComponentModel.DataAnnotations;

namespace TicketManagementSystem.Application.Features.Auth.Login;

public sealed record LoginCommand : IRequest<LoginResponse>
{
    [Required]
    [EmailAddress]
    public string Email {get; init;} = string.Empty;

    [Required]
    public string Password { get; init; } = string.Empty;
}