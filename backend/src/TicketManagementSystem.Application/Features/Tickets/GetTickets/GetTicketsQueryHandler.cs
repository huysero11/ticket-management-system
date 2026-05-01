using MediatR;
using TicketManagementSystem.Application.Abstractions.Repositories;
using TicketManagementSystem.Application.Abstractions.Security;
using TicketManagementSystem.Application.Common.Models;

namespace TicketManagementSystem.Application.Features.Tickets.GetTickets;

public sealed class GetTicketsQueryHandler 
    : IRequestHandler<GetTicketsQuery, PagedResult<TicketDto>>
{
    private readonly ITicketRepository _repository;
    private readonly ICurrentUserService _currentUserService;

    public GetTicketsQueryHandler(ITicketRepository repository, ICurrentUserService currentUserService)
    {
        _repository = repository;
        _currentUserService = currentUserService;
    }

    public async Task<PagedResult<TicketDto>> Handle(GetTicketsQuery request, CancellationToken cancellationToken)
    {
        var (tickets, totalCount) = await _repository.GetPagedAsync(
            request,
            _currentUserService.UserId,
            _currentUserService.Role,
            cancellationToken);
        var items = tickets.Select(ticket => new TicketDto(
            ticket.Id,
            ticket.Title,
            ticket.Description,
            ticket.Category.Name,
            ticket.Status,
            ticket.Priority,
            ticket.CreatedByUserId,
            ticket.AssignedToUserId,
            ticket.CreatedAtUtc,
            ticket.UpdatedAtUtc
        )).ToList();

        return new PagedResult<TicketDto>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}