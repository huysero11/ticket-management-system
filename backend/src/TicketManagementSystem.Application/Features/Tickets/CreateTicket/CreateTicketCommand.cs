using MediatR;
using TicketManagementSystem.Domain.Common;

namespace TicketManagementSystem.Application.Features.Tickets.CreateTicket;

public sealed record CreateTicketCommand(
    string Title,
    string Description,
    Guid CategoryId,
    TicketPriority Priority
) : IRequest<Guid>;