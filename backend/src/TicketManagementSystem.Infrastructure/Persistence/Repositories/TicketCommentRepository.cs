using Microsoft.EntityFrameworkCore;
using TicketManagementSystem.Application.Abstractions.Repositories;
using TicketManagementSystem.Domain.Entities;

namespace TicketManagementSystem.Infrastructure.Persistence.Repositories;

public sealed class TicketCommentRepository : ITicketCommentRepository
{
    private readonly ApplicationDbContext _dbContext;

    public TicketCommentRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(TicketComment comment, CancellationToken cancellationToken)
    {
        await _dbContext.TicketComments.AddAsync(comment, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<TicketComment>> GetByTicketIdAsync(Guid ticketId, CancellationToken cancellationToken)
    {
        return await _dbContext.TicketComments
            .AsNoTracking()
            .Include(x => x.User)
            .Where(x => x.TicketId == ticketId)
            .OrderBy(x => x.CreatedAtUtc)
            .ToListAsync(cancellationToken);
    }
}