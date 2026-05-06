using MediatR;
using TicketManagementSystem.Application.Abstractions.Repositories;
using TicketManagementSystem.Application.Abstractions.Security;
using TicketManagementSystem.Domain.Common;
using TicketManagementSystem.Domain.Entities;

namespace TicketManagementSystem.Application.Features.Tickets.ChangeTicketStatus;

public sealed class ChangeTicketStatusCommandHandler : IRequestHandler<ChangeTicketStatusCommand>
{
    private readonly ITicketRepository _ticketRepository;
    private readonly ICurrentUserService _currentUserService;

    public ChangeTicketStatusCommandHandler(
        ITicketRepository ticketRepository,
        ICurrentUserService currentUserService)
    {
        _ticketRepository = ticketRepository;
        _currentUserService = currentUserService;
    }

    public async Task Handle(
        ChangeTicketStatusCommand command,
        CancellationToken cancellationToken)
    {
        var ticket = await _ticketRepository.GetByIdAsync(
            command.TicketId,
            cancellationToken);

        if (ticket is null)
        {
            throw new KeyNotFoundException("Ticket not found.");
        }

        EnsureCurrentUserCanChangeStatus(ticket, command.Status);

        ticket.ChangeStatus(command.Status);

        await _ticketRepository.UpdateAsync(ticket, cancellationToken);
    }

    private void EnsureCurrentUserCanChangeStatus(
        Ticket ticket,
        TicketStatus newStatus)
    {
        var currentUserId = _currentUserService.UserId;
        var currentUserRole = _currentUserService.Role;

        if (currentUserRole == UserRole.Admin)
        {
            return;
        }

        if (currentUserRole == UserRole.Agent)
        {
            EnsureAgentCanChangeStatus(ticket, newStatus, currentUserId);
            return;
        }

        if (currentUserRole == UserRole.User)
        {
            EnsureUserCanChangeStatus(ticket, newStatus, currentUserId);
            return;
        }

        throw new UnauthorizedAccessException("You are not allowed to change ticket status.");
    }

    private static void EnsureAgentCanChangeStatus(
        Ticket ticket,
        TicketStatus newStatus,
        Guid currentUserId)
    {
        if (ticket.AssignedToUserId != currentUserId)
        {
            throw new UnauthorizedAccessException(
                "Agent can only change status of assigned tickets.");
        }

        var isAllowed = ticket.Status switch
        {
            TicketStatus.Open =>
                newStatus == TicketStatus.InProgress,

            TicketStatus.InProgress =>
                newStatus == TicketStatus.Resolved,

            TicketStatus.Resolved =>
                newStatus == TicketStatus.InProgress,

            _ => false
        };

        if (!isAllowed)
        {
            throw new UnauthorizedAccessException(
                $"Agent cannot change ticket status from {ticket.Status} to {newStatus}.");
        }
    }

    private static void EnsureUserCanChangeStatus(
        Ticket ticket,
        TicketStatus newStatus,
        Guid currentUserId)
    {
        if (ticket.CreatedByUserId != currentUserId)
        {
            throw new UnauthorizedAccessException(
                "User can only change status of their own tickets.");
        }

        var isAllowed = ticket.Status switch
        {
            TicketStatus.Resolved =>
                newStatus == TicketStatus.Closed,

            _ => false
        };

        if (!isAllowed)
        {
            throw new UnauthorizedAccessException(
                $"User cannot change ticket status from {ticket.Status} to {newStatus}.");
        }
    }
}