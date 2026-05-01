using MediatR;
using TicketManagementSystem.Application.Abstractions.Repositories;
using TicketManagementSystem.Application.Abstractions.Security;
using TicketManagementSystem.Domain.Common;

namespace TicketManagementSystem.Application.Features.Tickets.AssignTicket;

public sealed class AssignTicketCommandHandler : IRequestHandler<AssignTicketCommand>
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;

    public AssignTicketCommandHandler(
        ITicketRepository ticketRepository,
        IUserRepository userRepository,
        ICurrentUserService currentUserService)
    {
        _ticketRepository = ticketRepository;
        _userRepository = userRepository;
        _currentUserService = currentUserService;
    }

    public async Task Handle(AssignTicketCommand command, CancellationToken cancellationToken)
    {
        if (_currentUserService.Role is not UserRole.Admin and not UserRole.Agent)
        {
            throw new UnauthorizedAccessException("Only Admins and Agents can assign tickets.");
        }

        // check if ticket exists
        var ticket = await _ticketRepository.GetByIdAsync(command.TicketId, cancellationToken);
        if (ticket is null)
        {
            throw new KeyNotFoundException("Ticket not found.");
        }

        // check if user exists
        var assignedUser = await _userRepository.GetByIdAsync(command.AssignedToUserId, cancellationToken);
        if (assignedUser is null)
        {
            throw new KeyNotFoundException("Assigned user not found.");
        }

        // check assigned user role
        if (assignedUser.Role is not UserRole.Agent)
        {
            throw new InvalidOperationException("Assigned user must be an Agent.");
        }

        // assign ticket
        ticket.Assign(command.AssignedToUserId);
        await _ticketRepository.UpdateAsync(ticket, cancellationToken);
    }
}