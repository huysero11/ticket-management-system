using Microsoft.EntityFrameworkCore;
using TicketManagementSystem.Application.Abstractions.Dashboard;
using TicketManagementSystem.Application.Abstractions.Security;
using TicketManagementSystem.Application.Features.Dashboard.GetDashboardSummary;
using TicketManagementSystem.Domain.Common;
using TicketManagementSystem.Infrastructure.Persistence;

namespace TicketManagementSystem.Infrastructure.Dashboard;

public sealed class DashboardQueryService : IDashboardQueryService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;

    public DashboardQueryService(
        ApplicationDbContext dbContext,
        ICurrentUserService currentUserService)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
    }

    public async Task<DashboardSummaryResponse> GetSummaryAsync(
        CancellationToken cancellationToken)
    {
        var query = _dbContext.Tickets
            .AsNoTracking()
            .Include(ticket => ticket.Category)
            .AsQueryable();

        query = ApplyRoleFilter(query);

        var totalTickets = await query
            .CountAsync(cancellationToken);

        var openTickets = await query
            .CountAsync(
                ticket => ticket.Status == TicketStatus.Open,
                cancellationToken);

        var inProgressTickets = await query
            .CountAsync(
                ticket => ticket.Status == TicketStatus.InProgress,
                cancellationToken);

        var resolvedTickets = await query
            .CountAsync(
                ticket => ticket.Status == TicketStatus.Resolved,
                cancellationToken);

        var closedTickets = await query
            .CountAsync(
                ticket => ticket.Status == TicketStatus.Closed,
                cancellationToken);

        var recentTickets = await query
            .OrderByDescending(ticket => ticket.CreatedAtUtc)
            .Take(5)
            .Select(ticket => new DashboardRecentTicketDto(
                ticket.Id,
                ticket.Title,
                ticket.Category.Name,
                ticket.Status,
                ticket.Priority,
                ticket.CreatedByUserId,
                ticket.AssignedToUserId,
                ticket.CreatedAtUtc))
            .ToListAsync(cancellationToken);

        return new DashboardSummaryResponse(
            totalTickets,
            openTickets,
            inProgressTickets,
            resolvedTickets,
            closedTickets,
            recentTickets);
    }

    private IQueryable<Domain.Entities.Ticket> ApplyRoleFilter(
        IQueryable<Domain.Entities.Ticket> query)
    {
        var role = _currentUserService.Role;
        var userId = _currentUserService.UserId;

        return role switch
        {
            UserRole.Admin => query,

            UserRole.Agent => query
                .Where(ticket => ticket.AssignedToUserId == userId),

            UserRole.User => query
                .Where(ticket => ticket.CreatedByUserId == userId),

            _ => query.Where(ticket => false)
        };
    }
}