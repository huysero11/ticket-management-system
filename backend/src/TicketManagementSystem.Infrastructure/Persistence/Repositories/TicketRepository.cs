using Microsoft.EntityFrameworkCore;
using TicketManagementSystem.Application.Abstractions.Repositories;
using TicketManagementSystem.Application.Features.Tickets.GetTickets;
using TicketManagementSystem.Domain.Common;
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

    public async Task UpdateAsync(Ticket ticket, CancellationToken cancellationToken)
    {
        _dbContext.Tickets.Update(ticket);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<(IReadOnlyList<Ticket>, int TotalCount)> GetPagedAsync(
        GetTicketsQuery query,
        Guid currentUserId,
        UserRole currentUserRole,
        CancellationToken cancellationToken)
    {
        IQueryable<Ticket> ticketsQuery = _dbContext.Tickets
            .AsNoTracking()
            .Include(x => x.Category);
        
        ticketsQuery = currentUserRole switch
        {
            UserRole.Admin => ticketsQuery,
            UserRole.Agent => ticketsQuery.Where(t => t.AssignedToUserId == currentUserId),
            UserRole.User => ticketsQuery.Where(t => t.CreatedByUserId == currentUserId),
            _ => ticketsQuery.Where(t => false)
        };

        // Filters
        if (query.Status.HasValue)
        {
            ticketsQuery = ticketsQuery.Where(t => t.Status == query.Status.Value);
        }

        if (query.Priority.HasValue)
        {
            ticketsQuery = ticketsQuery.Where(t => t.Priority == query.Priority.Value);
        }

        if (query.AssignedToUserId.HasValue)
        {
            ticketsQuery = ticketsQuery.Where(t => t.AssignedToUserId == query.AssignedToUserId.Value);
        }

        // count
        var totalCount = await ticketsQuery.CountAsync(cancellationToken);

        // pagination
        var pageNumber = query.PageNumber < 1 ? 1 : query.PageNumber;
        var pageSize = query.PageSize < 1 ? 10 : query.PageSize;
        pageSize = pageSize > 100 ? 100 : pageSize;

        var tickets = await ticketsQuery
            .OrderByDescending(x => x.CreatedAtUtc)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (tickets, totalCount);
    }
}