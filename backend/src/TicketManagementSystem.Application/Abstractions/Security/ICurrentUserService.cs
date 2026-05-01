using TicketManagementSystem.Domain.Common;
using TicketManagementSystem.Domain.Entities;

namespace TicketManagementSystem.Application.Abstractions.Security;

public interface ICurrentUserService
{
    Guid UserId { get; }
    UserRole Role { get; }
    bool IsAuthenticated { get; }
}