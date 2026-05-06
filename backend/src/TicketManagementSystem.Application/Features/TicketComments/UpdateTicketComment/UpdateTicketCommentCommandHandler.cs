using MediatR;
using TicketManagementSystem.Application.Abstractions.Repositories;
using TicketManagementSystem.Application.Abstractions.Security;
using TicketManagementSystem.Domain.Common;

namespace TicketManagementSystem.Application.Features.TicketComments.UpdateTicketComment;

public sealed class UpdateTicketCommentCommandHandler
    : IRequestHandler<UpdateTicketCommentCommand, UpdateTicketCommentResponse>
{
    private readonly ITicketCommentRepository _ticketCommentRepository;
    private readonly ICurrentUserService _currentUserService;

    public UpdateTicketCommentCommandHandler(
        ITicketCommentRepository ticketCommentRepository,
        ICurrentUserService currentUserService)
    {
        _ticketCommentRepository = ticketCommentRepository;
        _currentUserService = currentUserService;
    }

    public async Task<UpdateTicketCommentResponse> Handle(
        UpdateTicketCommentCommand command,
        CancellationToken cancellationToken)
    {
        var comment = await _ticketCommentRepository.GetByIdAndTicketIdAsync(
            command.CommentId,
            command.TicketId,
            cancellationToken
        );
        if (comment is null)
        {
            throw new KeyNotFoundException("Ticket comment not found.");
        }

        if (comment.UserId != _currentUserService.UserId)
        {
            throw new UnauthorizedAccessException("You are not authorized to update this comment.");
        }

        if (comment.Ticket.Status == TicketStatus.Closed)
        {
            throw new InvalidOperationException("Cannot update comment on a closed ticket.");
        }

        // update
        comment.UpdateMessage(command.Message);
        await _ticketCommentRepository.UpdateAsync(comment, cancellationToken);
        
        return new UpdateTicketCommentResponse(
            comment.Id,
            comment.TicketId,
            comment.UserId,
            comment.Message,
            comment.CreatedAtUtc,
            comment.UpdatedAtUtc
        );
    }
}