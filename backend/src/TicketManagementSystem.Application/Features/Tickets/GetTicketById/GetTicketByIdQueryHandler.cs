using MediatR;
using TicketManagementSystem.Application.Abstractions.Repositories;

namespace TicketManagementSystem.Application.Features.Tickets.GetTicketById;

public sealed class GetTicketByIdQueryHandler
    : IRequestHandler<GetTicketByIdQuery, TicketDto>
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IUserRepository _userRepository;

    public GetTicketByIdQueryHandler(
        ITicketRepository ticketRepository,
        IUserRepository userRepository)
    {
        _ticketRepository = ticketRepository;
        _userRepository = userRepository;
    }

    public async Task<TicketDto> Handle(
        GetTicketByIdQuery request,
        CancellationToken cancellationToken)
    {
        var ticket = await _ticketRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (ticket is null)
        {
            throw new KeyNotFoundException("Ticket not found.");
        }

        string? assignedToUserName = null;

        if (ticket.AssignedToUserId.HasValue)
        {
            var assignedUsers = await _userRepository.GetByIdsAsync(
                new[] { ticket.AssignedToUserId.Value },
                cancellationToken);

            assignedToUserName = assignedUsers
                .FirstOrDefault()
                ?.FullName;
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
    }
}