using MediatR;

namespace TicketManagementSystem.Application.Features.Dashboard.GetDashboardSummary;

public sealed record GetDashboardSummaryQuery : IRequest<DashboardSummaryResponse>;