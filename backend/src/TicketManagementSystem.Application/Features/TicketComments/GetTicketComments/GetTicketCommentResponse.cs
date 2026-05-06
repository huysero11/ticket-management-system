using TicketManagementSystem.Domain.Common;

namespace TicketManagementSystem.Application.Features.TicketComments.GetTicketComments;

public sealed record GetTicketCommentResponse(
    Guid Id,
    Guid TicketId,
    Guid UserId,
    string AuthorName,
    UserRole AuthorRole,
    string Message,
    DateTime CreatedAtUtc
);