namespace TicketManagementSystem.Application.Features.TicketComments.AddTicketComment;

public sealed record AddTicketCommentResponse(
    Guid Id,
    Guid TicketId,
    Guid UserId,
    string Message,
    DateTime CreatedAtUtc
);