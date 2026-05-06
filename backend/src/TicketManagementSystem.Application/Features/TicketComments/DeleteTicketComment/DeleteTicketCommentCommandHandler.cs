using MediatR;
using TicketManagementSystem.Application.Abstractions.Repositories;
using TicketManagementSystem.Application.Abstractions.Security;
using TicketManagementSystem.Domain.Common;

namespace TicketManagementSystem.Application.Features.TicketComments.DeleteTicketComment;

public sealed class DeleteTicketCommentCommandHandler
    : IRequestHandler<DeleteTicketCommentCommand>
{
    private readonly ITicketCommentRepository _ticketCommentRepository;
    private readonly ICurrentUserService _currentUserService;

    public DeleteTicketCommentCommandHandler(
        ITicketCommentRepository ticketCommentRepository,
        ICurrentUserService currentUserService)
    {
        _ticketCommentRepository = ticketCommentRepository;
        _currentUserService = currentUserService;
    }

    public async Task Handle(
        DeleteTicketCommentCommand command,
        CancellationToken cancellationToken)
    {
        var comment = await _ticketCommentRepository.GetByIdAndTicketIdAsync(
            command.CommentId,
            command.TicketId,
            cancellationToken);

        if (comment is null)
        {
            throw new KeyNotFoundException("Comment not found.");
        }

        if (comment.UserId != _currentUserService.UserId)
        {
            throw new UnauthorizedAccessException("Only the author can delete this comment.");
        }

        if (comment.Ticket.Status == TicketStatus.Closed)
        {
            throw new InvalidOperationException("Cannot delete comment on a closed ticket.");
        }

        comment.SoftDelete(_currentUserService.UserId);

        await _ticketCommentRepository.UpdateAsync(comment, cancellationToken);
    }
}