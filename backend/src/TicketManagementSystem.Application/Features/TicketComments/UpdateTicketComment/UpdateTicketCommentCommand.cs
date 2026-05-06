using MediatR;

namespace TicketManagementSystem.Application.Features.TicketComments.UpdateTicketComment;

public sealed record UpdateTicketCommentCommand(
    Guid TicketId,
    Guid CommentId,
    string Message
) : IRequest<UpdateTicketCommentResponse>;