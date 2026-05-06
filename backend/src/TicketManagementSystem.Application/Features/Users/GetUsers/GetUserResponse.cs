using TicketManagementSystem.Domain.Common;

namespace TicketManagementSystem.Application.Features.Users.GetUsers;

public sealed record GetUserResponse(
    Guid Id,
    string Email,
    string FullName,
    UserRole Role
);