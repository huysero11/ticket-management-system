using TicketManagementSystem.Domain.Entities;

namespace TicketManagementSystem.Application.Abstractions.Repositories;

public interface ITicketRepository
{
    Task AddAsync(Ticket ticket, CancellationToken cancellationToken);
    Task<Ticket?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}