using TicketManagementSystem.Domain.Common;

namespace TicketManagementSystem.WebAPI.Contracts.Tickets;

public sealed record ChangeTicketStatusRequest(
    TicketStatus Status
);