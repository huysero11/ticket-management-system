using MediatR;
using TicketManagementSystem.Application.Abstractions.Repositories;
using TicketManagementSystem.Application.Abstractions.Security;
using TicketManagementSystem.Domain.Common;
using TicketManagementSystem.Domain.Entities;

namespace TicketManagementSystem.Application.Features.TicketComments.AddTicketComment;

public sealed class AddTicketCommentCommandHandler
    : IRequestHandler<AddTicketCommentCommand, AddTicketCommentResponse>
{
    private readonly ITicketRepository _ticketRepository;
    private readonly ITicketCommentRepository _ticketCommentRepository;
    private readonly ICurrentUserService _currentUserService;

    public AddTicketCommentCommandHandler(
        ITicketRepository ticketRepository,
        ITicketCommentRepository ticketCommentRepository,
        ICurrentUserService currentUserService)
    {
        _ticketRepository = ticketRepository;
        _ticketCommentRepository = ticketCommentRepository;
        _currentUserService = currentUserService;
    }

    public async Task<AddTicketCommentResponse> Handle(
        AddTicketCommentCommand command,
        CancellationToken cancellationToken)
    {
        var ticket = await _ticketRepository.GetByIdAsync(
            command.TicketId,
            cancellationToken);
        if (ticket is null)
        {
            throw new KeyNotFoundException("Ticket not found.");
        }

        EnsureCurrentUserCanAccessTicket(ticket);

        if (ticket.Status == TicketStatus.Closed)
        {
            throw new InvalidOperationException("Cannot add comment to a closed ticket.");
        }

        var comment = TicketComment.Create(
            ticket.Id,
            _currentUserService.UserId,
            command.Message);

        await _ticketCommentRepository.AddAsync(comment, cancellationToken);

        return new AddTicketCommentResponse(
            comment.Id,
            comment.TicketId,
            comment.UserId,
            comment.Message,
            comment.CreatedAtUtc);
    }

    private void EnsureCurrentUserCanAccessTicket(Ticket ticket)
    {
        var currentUserId = _currentUserService.UserId;
        var currentUserRole = _currentUserService.Role;

        if (currentUserRole == UserRole.Admin)
        {
            return;
        }

        if (currentUserRole == UserRole.Agent &&
            ticket.AssignedToUserId == currentUserId)
        {
            return;
        }

        if (currentUserRole == UserRole.User &&
            ticket.CreatedByUserId == currentUserId)
        {
            return;
        }

        throw new UnauthorizedAccessException("You are not allowed to comment on this ticket.");
    }
}