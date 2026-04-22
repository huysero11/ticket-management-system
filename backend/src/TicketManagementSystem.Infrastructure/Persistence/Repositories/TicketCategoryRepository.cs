using Microsoft.EntityFrameworkCore;
using TicketManagementSystem.Application.Abstractions.Repositories;
using TicketManagementSystem.Domain.Entities;

namespace TicketManagementSystem.Infrastructure.Persistence.Repositories;

public sealed class TicketCategoryRepository : ITicketCategoryRepository
{
    private readonly ApplicationDbContext _dbContext;

    public TicketCategoryRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken)
    {
        return await _dbContext.TicketCategories
            .AnyAsync(tc => tc.Name == name, cancellationToken);
    }

    public async Task AddAsync(TicketCategory ticketCategory, CancellationToken cancellationToken)
    {
        await _dbContext.TicketCategories.AddAsync(ticketCategory, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<TicketCategory>> GetAllAsync(
        bool includeInactive,
        CancellationToken cancellationToken
    )
    {
        var query = _dbContext.TicketCategories.AsNoTracking();
        if (!includeInactive)
        {
            query = query.Where(tc => tc.IsActive);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<TicketCategory?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.TicketCategories
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}