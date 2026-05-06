namespace TicketManagementSystem.Application.Features.TicketComments.UpdateTicketComment;

public sealed record UpdateTicketCommentResponse(
    Guid Id,
    Guid TicketId,
    Guid UserId,
    string Message,
    DateTime CreatedAtUtc,
    DateTime? UpdatedAtUtc
);