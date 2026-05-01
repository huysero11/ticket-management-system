namespace TicketManagementSystem.WebAPI.Contracts.Tickets;

public sealed record AssignTicketRequest(
    Guid AssignedToUserId
);