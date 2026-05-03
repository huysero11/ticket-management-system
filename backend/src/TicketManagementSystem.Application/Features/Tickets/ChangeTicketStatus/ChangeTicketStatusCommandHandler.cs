using MediatR;
using TicketManagementSystem.Application.Abstractions.Repositories;
using TicketManagementSystem.Application.Abstractions.Security;
using TicketManagementSystem.Domain.Common;

namespace TicketManagementSystem.Application.Features.Tickets.ChangeTicketStatus;

public sealed class ChangeTicketStatusCommandHandler : IRequestHandler<ChangeTicketStatusCommand>
{
    private readonly ITicketRepository _ticketRepository;
    private readonly ICurrentUserService _currentUserService;

    public ChangeTicketStatusCommandHandler(ITicketRepository ticketRepository, ICurrentUserService currentUserService)
    {
        _ticketRepository = ticketRepository;
        _currentUserService = currentUserService;
    }

    public async Task Handle(ChangeTicketStatusCommand command, CancellationToken cancellationToken)
    {
        if (_currentUserService.Role == UserRole.User)
        {
            throw new UnauthorizedAccessException("Only admin and support can change ticket status.");
        }

        var ticket = await _ticketRepository.GetByIdAsync(command.TicketId, cancellationToken);
        if (ticket is null)
        {
            throw new KeyNotFoundException("Ticket not found.");
        }

        ticket.ChangeStatus(command.Status);
        await _ticketRepository.UpdateAsync(ticket, cancellationToken);
    }
}