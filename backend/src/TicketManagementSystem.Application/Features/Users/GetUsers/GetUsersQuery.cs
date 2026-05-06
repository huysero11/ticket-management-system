using MediatR;
using TicketManagementSystem.Domain.Common;

namespace TicketManagementSystem.Application.Features.Users.GetUsers;

public sealed record GetUsersQuery(
    UserRole? Role
) : IRequest<IReadOnlyList<GetUserResponse>>;