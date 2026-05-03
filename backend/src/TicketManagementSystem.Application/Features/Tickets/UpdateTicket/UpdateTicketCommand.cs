using MediatR;
using TicketManagementSystem.Domain.Common;

namespace TicketManagementSystem.Application.Features.Tickets.UpdateTicket;
public sealed record UpdateTicketCommand(
    Guid TicketId,
    string Title,
    string Description, 
    TicketPriority Priority,
    Guid CategoryId
) : IRequest;