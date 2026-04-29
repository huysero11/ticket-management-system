using Microsoft.EntityFrameworkCore;
using TicketManagementSystem.Application.Abstractions.Repositories;
using TicketManagementSystem.Domain.Entities;

namespace TicketManagementSystem.Infrastructure.Persistence.Repositories;

public sealed class TicketRepository : ITicketRepository
{
    private readonly ApplicationDbContext _dbContext;

    public TicketRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Ticket ticket, CancellationToken cancellationToken)
    {
        await _dbContext.Tickets.AddAsync(ticket, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Ticket?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Tickets
            .Include(x => x.Category) 
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}