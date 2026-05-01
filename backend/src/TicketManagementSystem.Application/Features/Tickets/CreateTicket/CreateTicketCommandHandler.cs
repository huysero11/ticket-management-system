using MediatR;
using TicketManagementSystem.Application.Abstractions.Repositories;
using TicketManagementSystem.Application.Abstractions.Security;
using TicketManagementSystem.Domain.Entities;

namespace TicketManagementSystem.Application.Features.Tickets.CreateTicket;

public sealed class CreateTicketCommandHandler : IRequestHandler<CreateTicketCommand, Guid>
{
    private readonly ITicketRepository _ticketRepository;
    private readonly ITicketCategoryRepository _categoryRepository;
    private readonly ICurrentUserService _currentUserService;

    public CreateTicketCommandHandler(
        ITicketRepository ticketRepository,
        ITicketCategoryRepository categoryRepository,
        ICurrentUserService currentUserService)
    {
        _ticketRepository = ticketRepository;
        _categoryRepository = categoryRepository;
        _currentUserService = currentUserService;
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
            _currentUserService.UserId
        );

        await _ticketRepository.AddAsync(ticket, cancellationToken);

        return ticket.Id;
    }
}