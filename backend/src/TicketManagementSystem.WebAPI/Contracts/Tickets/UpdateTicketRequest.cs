using TicketManagementSystem.Domain.Common;

namespace TicketManagementSystem.WebAPI.Contracts.Tickets;

public sealed record UpdateTicketRequest(
    string Title,
    string Description,
    TicketPriority Priority,
    Guid CategoryId
);