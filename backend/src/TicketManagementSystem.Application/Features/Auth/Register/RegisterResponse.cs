namespace TicketManagementSystem.Application.Features.Auth.Register;

public sealed record RegisterResponse(
    Guid Id,
    string Email,
    string FullName
);