using Microsoft.EntityFrameworkCore;
using TicketManagementSystem.Application.Abstractions.Repositories;
using TicketManagementSystem.Domain.Common;
using TicketManagementSystem.Domain.Entities;

namespace TicketManagementSystem.Infrastructure.Persistence.Repositories;

public sealed class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _dbContext;

    public UserRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<bool> ExistsByEmailAsync(
        string email,
        CancellationToken cancellationToken)
    {
        return _dbContext.Users.AnyAsync(
            user => user.Email == email,
            cancellationToken);
    }

    public async Task AddAsync(
        User user,
        CancellationToken cancellationToken)
    {
        await _dbContext.Users.AddAsync(user, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<User?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken)
    {
        return _dbContext.Users.FirstOrDefaultAsync(
            user => user.Email == email,
            cancellationToken);
    }

    public Task<User?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return _dbContext.Users.FirstOrDefaultAsync(
            user => user.Id == id,
            cancellationToken);
    }

    public async Task<IReadOnlyList<User>> GetUsersAsync(
        UserRole? role,
        CancellationToken cancellationToken)
    {
        var query = _dbContext.Users
            .AsNoTracking()
            .AsQueryable();

        if (role.HasValue)
        {
            query = query.Where(user => user.Role == role.Value);
        }

        return await query
            .OrderBy(user => user.FullName)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<User>> GetByIdsAsync(
        IReadOnlyCollection<Guid> ids,
        CancellationToken cancellationToken)
    {
        if (ids.Count == 0)
        {
            return [];
        }

        return await _dbContext.Users
            .AsNoTracking()
            .Where(user => ids.Contains(user.Id))
            .ToListAsync(cancellationToken);
    }
}