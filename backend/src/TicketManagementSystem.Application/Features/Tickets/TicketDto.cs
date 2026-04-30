using TicketManagementSystem.Domain.Common;

namespace TicketManagementSystem.Application.Features.Tickets;

public sealed record TicketDto(
    Guid Id,
    string Title,
    string Description,
    string CategoryName,
    TicketStatus Status,
    TicketPriority Priority,
    Guid CreatedByUserId,
    Guid? AssignedToUserId,
    DateTime CreatedAtUtc,
    DateTime? UpdatedAtUtc
);