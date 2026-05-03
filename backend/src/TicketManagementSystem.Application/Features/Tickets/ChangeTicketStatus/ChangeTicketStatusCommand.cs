using MediatR;
using TicketManagementSystem.Domain.Common;

namespace TicketManagementSystem.Application.Features.Tickets.ChangeTicketStatus;

public sealed record ChangeTicketStatusCommand(
    Guid TicketId,
    TicketStatus Status
) : IRequest;