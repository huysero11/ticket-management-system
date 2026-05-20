using MediatR;
using TicketManagementSystem.Application.Abstractions.Repositories;
using TicketManagementSystem.Application.Abstractions.Security;
using TicketManagementSystem.Application.Common.Models;

namespace TicketManagementSystem.Application.Features.Tickets.GetTickets;

public sealed class GetTicketsQueryHandler
    : IRequestHandler<GetTicketsQuery, PagedResult<TicketDto>>
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetTicketsQueryHandler(
        ITicketRepository ticketRepository,
        IUserRepository userRepository,
        ICurrentUserService currentUserService)
    {
        _ticketRepository = ticketRepository;
        _userRepository = userRepository;
        _currentUserService = currentUserService;
    }

    public async Task<PagedResult<TicketDto>> Handle(
        GetTicketsQuery request,
        CancellationToken cancellationToken)
    {
        var (tickets, totalCount) = await _ticketRepository.GetPagedAsync(
            request,
            _currentUserService.UserId,
            _currentUserService.Role,
            cancellationToken);

        var assignedUserIds = tickets
            .Where(ticket => ticket.AssignedToUserId.HasValue)
            .Select(ticket => ticket.AssignedToUserId!.Value)
            .Distinct()
            .ToList();

        var assignedUsers = await _userRepository.GetByIdsAsync(
            assignedUserIds,
            cancellationToken);

        var assignedUserNameById = assignedUsers
            .ToDictionary(
                user => user.Id,
                user => user.FullName);

        var items = tickets
            .Select(ticket =>
            {
                string? assignedToUserName = null;

                if (ticket.AssignedToUserId.HasValue)
                {
                    assignedUserNameById.TryGetValue(
                        ticket.AssignedToUserId.Value,
                        out assignedToUserName);
                }

                return new TicketDto(
                    ticket.Id,
                    ticket.Title,
                    ticket.Description,
                    ticket.Category.Name,
                    ticket.Status,
                    ticket.Priority,
                    ticket.CreatedByUserId,
                    ticket.AssignedToUserId,
                    assignedToUserName,
                    ticket.CreatedAtUtc,
                    ticket.UpdatedAtUtc);
            })
            .ToList();

        return new PagedResult<TicketDto>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}