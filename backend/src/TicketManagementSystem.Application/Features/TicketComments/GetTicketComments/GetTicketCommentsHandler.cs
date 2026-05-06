using MediatR;
using TicketManagementSystem.Application.Abstractions.Repositories;
using TicketManagementSystem.Application.Abstractions.Security;
using TicketManagementSystem.Domain.Common;
using TicketManagementSystem.Domain.Entities;

namespace TicketManagementSystem.Application.Features.TicketComments.GetTicketComments;

public sealed class GetTicketCommentsQueryHandler
    : IRequestHandler<GetTicketCommentsQuery, IReadOnlyList<GetTicketCommentResponse>>
{
    private readonly ITicketRepository _ticketRepository;
    private readonly ITicketCommentRepository _ticketCommentRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetTicketCommentsQueryHandler(
        ITicketRepository ticketRepository,
        ITicketCommentRepository ticketCommentRepository,
        ICurrentUserService currentUserService)
    {
        _ticketRepository = ticketRepository;
        _ticketCommentRepository = ticketCommentRepository;
        _currentUserService = currentUserService;
    }

    public async Task<IReadOnlyList<GetTicketCommentResponse>> Handle(
        GetTicketCommentsQuery query,
        CancellationToken cancellationToken)
    {
        var ticket = await _ticketRepository.GetByIdAsync(
            query.TicketId,
            cancellationToken);

        if (ticket is null)
        {
            throw new KeyNotFoundException("Ticket not found.");
        }

        EnsureCurrentUserCanAccessTicket(ticket);

        var comments = await _ticketCommentRepository.GetByTicketIdAsync(
            ticket.Id,
            cancellationToken);

        return comments
            .Select(comment => new GetTicketCommentResponse(
                comment.Id,
                comment.TicketId,
                comment.UserId,
                comment.User.FullName,
                comment.User.Role,
                comment.Message,
                comment.CreatedAtUtc))
            .ToList();
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

        throw new UnauthorizedAccessException("You are not allowed to view comments on this ticket.");
    }
}