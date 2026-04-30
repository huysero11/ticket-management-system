using TicketManagementSystem.Application.Features.Tickets.GetTickets;
using TicketManagementSystem.Domain.Entities;

namespace TicketManagementSystem.Application.Abstractions.Repositories;

public interface ITicketRepository
{
    Task AddAsync(Ticket ticket, CancellationToken cancellationToken);
    Task<Ticket?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<(IReadOnlyList<Ticket>, int TotalCount)> GetPagedAsync(
        GetTicketsQuery query,
        CancellationToken cancellationToken);
}