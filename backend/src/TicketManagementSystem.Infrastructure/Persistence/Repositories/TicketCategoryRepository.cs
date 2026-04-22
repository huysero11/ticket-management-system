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
}