using MediatR;

namespace TicketManagementSystem.Application.Features.TicketComments.DeleteTicketComment;

public sealed record DeleteTicketCommentCommand(
    Guid TicketId,
    Guid CommentId
) : IRequest;