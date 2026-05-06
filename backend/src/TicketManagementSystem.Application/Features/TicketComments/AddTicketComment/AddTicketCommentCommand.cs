using MediatR;

namespace TicketManagementSystem.Application.Features.TicketComments.AddTicketComment;

public sealed record AddTicketCommentCommand(
    Guid TicketId,
    string Message
) : IRequest<AddTicketCommentResponse>;