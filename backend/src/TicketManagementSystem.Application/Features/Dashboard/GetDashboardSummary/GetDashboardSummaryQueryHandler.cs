using MediatR;
using TicketManagementSystem.Application.Abstractions.Dashboard;

namespace TicketManagementSystem.Application.Features.Dashboard.GetDashboardSummary;

public sealed class GetDashboardSummaryQueryHandler
    : IRequestHandler<GetDashboardSummaryQuery, DashboardSummaryResponse>
{
    private readonly IDashboardQueryService _dashboardQueryService;

    public GetDashboardSummaryQueryHandler(IDashboardQueryService dashboardQueryService)
    {
        _dashboardQueryService = dashboardQueryService;
    }

    public Task<DashboardSummaryResponse> Handle(
        GetDashboardSummaryQuery request,
        CancellationToken cancellationToken)
    {
        return _dashboardQueryService.GetSummaryAsync(cancellationToken);
    }
}