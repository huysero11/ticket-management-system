using MediatR;
using TicketManagementSystem.Application.Abstractions.Repositories;
using TicketManagementSystem.Domain.Entities;

namespace TicketManagementSystem.Application.Features.Tickets.CreateTicket;

public sealed class CreateTicketCommandHandler : IRequestHandler<CreateTicketCommand, Guid>
{
    private readonly ITicketRepository _ticketRepository;
    private readonly ITicketCategoryRepository _categoryRepository;

    public CreateTicketCommandHandler(
        ITicketRepository ticketRepository,
        ITicketCategoryRepository categoryRepository)
    {
        _ticketRepository = ticketRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<Guid> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);
        if (category is null || !category.IsActive)
        {
            throw new InvalidOperationException($"Invalid Ticket Category with ID: {request.CategoryId}");
        }

        var ticket = Ticket.Create(
            request.Title,
            request.Description,
            request.CategoryId,
            request.Priority,
            request.CreatedByUserId
        );

        await _ticketRepository.AddAsync(ticket, cancellationToken);

        return ticket.Id;
    }
}