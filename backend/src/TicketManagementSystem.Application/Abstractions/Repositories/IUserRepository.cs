using TicketManagementSystem.Domain.Common;
using TicketManagementSystem.Domain.Entities;

namespace TicketManagementSystem.Application.Abstractions.Repositories;

public interface IUserRepository
{
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken);
    Task AddAsync(User user, CancellationToken cancellationToken);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<User>> GetUsersAsync(UserRole? role, CancellationToken cancellationToken);
}