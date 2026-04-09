using System.ComponentModel.DataAnnotations;
using MediatR;

namespace TicketManagementSystem.Application.Features.Auth.Register;

public sealed record RegisterCommand : IRequest<RegisterResponse>
{
    [Required]
    [MaxLength(100)]
    public string FullName {get; init;} = string.Empty;

    [Required]
    [EmailAddress]
    public string Email {get; init;} = string.Empty;

    [Required]
    [MinLength(6)]
    public string Password {get; init;} = string.Empty;

}