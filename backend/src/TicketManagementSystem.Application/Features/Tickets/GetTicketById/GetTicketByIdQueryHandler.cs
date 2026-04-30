using MediatR;
using TicketManagementSystem.Application.Abstractions.Repositories;

namespace TicketManagementSystem.Application.Features.Tickets.GetTicketById;

public sealed class GetTicketByIdQueryHandler : IRequestHandler<GetTicketByIdQuery, TicketDto>
{
    private readonly ITicketRepository _repository;

    public GetTicketByIdQueryHandler(ITicketRepository repository)
    {
        _repository = repository;
    }

    public async Task<TicketDto> Handle(GetTicketByIdQuery request, CancellationToken cancellationToken)
    {
        var ticket = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (ticket is null)
        {
            throw new Exception("Ticket not found");
        }

        return new TicketDto(
            ticket.Id,
            ticket.Title,
            ticket.Description,
            ticket.Category.Name,
            ticket.Status,
            ticket.Priority,
            ticket.CreatedByUserId,
            ticket.AssignedToUserId,
            ticket.CreatedAtUtc,
            ticket.UpdatedAtUtc
        );
    }
}