using Microsoft.EntityFrameworkCore;
using TicketManagementSystem.Application.Abstractions.Repositories;
using TicketManagementSystem.Domain.Entities;

namespace TicketManagementSystem.Infrastructure.Persistence.Repositories;

public sealed class TicketCommentRepository : ITicketCommentRepository
{
    private readonly ApplicationDbContext _context;

    public TicketCommentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        TicketComment comment,
        CancellationToken cancellationToken)
    {
        await _context.TicketComments.AddAsync(comment, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<TicketComment?> GetByIdAndTicketIdAsync(
        Guid commentId,
        Guid ticketId,
        CancellationToken cancellationToken)
    {
        return await _context.TicketComments
            .Include(x => x.Ticket)
            .FirstOrDefaultAsync(
                x => x.Id == commentId &&
                     x.TicketId == ticketId &&
                     !x.IsDeleted,
                cancellationToken);
    }

    public async Task<List<TicketComment>> GetByTicketIdAsync(
        Guid ticketId,
        CancellationToken cancellationToken)
    {
        return await _context.TicketComments
            .AsNoTracking()
            .Include(x => x.User)
            .Where(x => x.TicketId == ticketId && !x.IsDeleted)
            .OrderBy(x => x.CreatedAtUtc)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        TicketComment comment,
        CancellationToken cancellationToken)
    {
        _context.TicketComments.Update(comment);
        await _context.SaveChangesAsync(cancellationToken);
    }
}