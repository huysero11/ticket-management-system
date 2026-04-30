using MediatR;

namespace TicketManagementSystem.Application.Features.Tickets.GetTicketById;

public sealed record GetTicketByIdQuery(Guid Id) : IRequest<TicketDto>;