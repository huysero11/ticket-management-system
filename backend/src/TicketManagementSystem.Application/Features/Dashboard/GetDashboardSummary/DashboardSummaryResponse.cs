namespace TicketManagementSystem.Application.Features.Dashboard.GetDashboardSummary;

public sealed record DashboardSummaryResponse(
    int TotalTickets,
    int OpenTickets,
    int InProgressTickets,
    int ResolvedTickets,
    int ClosedTickets,
    IReadOnlyList<DashboardRecentTicketDto> RecentTickets
);