using TicketManagementSystem.Domain.Entities;

namespace TicketManagementSystem.Application.Abstractions.Repositories;

public interface ITicketCategoryRepository
{
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
    Task AddAsync(TicketCategory ticketCategory, CancellationToken cancellationToken = default);

}