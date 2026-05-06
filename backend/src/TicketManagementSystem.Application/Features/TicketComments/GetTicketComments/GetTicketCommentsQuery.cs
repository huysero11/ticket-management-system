using MediatR;

namespace TicketManagementSystem.Application.Features.TicketComments.GetTicketComments;

public sealed record GetTicketCommentsQuery(
    Guid TicketId
) : IRequest<IReadOnlyList<GetTicketCommentResponse>>;