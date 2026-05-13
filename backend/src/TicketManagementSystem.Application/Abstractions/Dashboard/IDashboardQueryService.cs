using TicketManagementSystem.Application.Features.Dashboard.GetDashboardSummary;

namespace TicketManagementSystem.Application.Abstractions.Dashboard;

public interface IDashboardQueryService
{
    Task<DashboardSummaryResponse> GetSummaryAsync(CancellationToken cancellationToken);
}