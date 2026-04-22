namespace TicketManagementSystem.Application.Features.Auth.GetMe;

public sealed record GetMeResponse(Guid Id, string Email, string FullName);