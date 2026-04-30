using MediatR;
using TicketManagementSystem.Application.Common.Models;
using TicketManagementSystem.Domain.Common;

namespace TicketManagementSystem.Application.Features.Tickets.GetTickets;

public sealed record GetTicketsQuery(
    int PageNumber = 1,
    int PageSize = 10,
    TicketStatus? Status = null,
    TicketPriority? Priority = null,
    Guid? AssignedToUserId = null
) : IRequest<PagedResult<TicketDto>>;