using TicketManagementSystem.Domain.Common;

namespace TicketManagementSystem.Application.Features.Dashboard.GetDashboardSummary;

public sealed record DashboardRecentTicketDto(
    Guid Id,
    string Title,
    string CategoryName,
    TicketStatus Status,
    TicketPriority Priority,
    Guid CreatedByUserId,
    Guid? AssignedToUserId,
    string? AssignedToUserName,
    DateTime CreatedAtUtc
);