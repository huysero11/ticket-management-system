using TicketManagementSystem.Domain.Entities;

namespace TicketManagementSystem.Application.Abstractions.Repositories;

public interface ITicketCommentRepository
{
    Task AddAsync(
        TicketComment comment,
        CancellationToken cancellationToken);

    Task<TicketComment?> GetByIdAndTicketIdAsync(
        Guid commentId,
        Guid ticketId,
        CancellationToken cancellationToken);

    Task<List<TicketComment>> GetByTicketIdAsync(
        Guid ticketId,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        TicketComment comment,
        CancellationToken cancellationToken);
}