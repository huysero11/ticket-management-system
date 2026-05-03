using MediatR;
using TicketManagementSystem.Application.Abstractions.Repositories;
using TicketManagementSystem.Application.Abstractions.Security;
using TicketManagementSystem.Domain.Common;

namespace TicketManagementSystem.Application.Features.Tickets.UpdateTicket;

public sealed class UpdateTicketCommandHandler : IRequestHandler<UpdateTicketCommand>
{
    private readonly ITicketRepository _ticketRepository;
    private readonly ITicketCategoryRepository _ticketCategoryRepository;
    private readonly ICurrentUserService _currentUserService;

    public UpdateTicketCommandHandler(
        ITicketRepository ticketRepository,
        ITicketCategoryRepository ticketCategoryRepository,
        ICurrentUserService currentUserService)
    {
        _ticketRepository = ticketRepository;
        _ticketCategoryRepository = ticketCategoryRepository;
        _currentUserService = currentUserService;
    }

    public async Task Handle(UpdateTicketCommand request, CancellationToken cancellationToken)
    {
        // validate ticket exists
        var ticket = await _ticketRepository.GetByIdAsync(request.TicketId, cancellationToken);
        if (ticket is null)
        {
            throw new KeyNotFoundException($"Ticket with id {request.TicketId} not found.");
        }

        // validate category exists
        var category = await _ticketCategoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);
        if (category is null)
        {
            throw new KeyNotFoundException($"Ticket category with id {request.CategoryId} not found.");
        }

        // authorization logic
        if (_currentUserService.Role == UserRole.User)
        {
            if (ticket.CreatedByUserId != _currentUserService.UserId)
            {
                throw new UnauthorizedAccessException("Users can only update their own tickets.");
            }
        }

        if (_currentUserService.Role == UserRole.Agent)
        {
            if (ticket.AssignedToUserId != _currentUserService.UserId)
            {
                throw new UnauthorizedAccessException("Agents can only update tickets assigned to them.");
            }
        }

        // update ticket
        ticket.Update(request.Title, request.Description, request.Priority, request.CategoryId);

        await _ticketRepository.UpdateAsync(ticket, cancellationToken);
    }
}