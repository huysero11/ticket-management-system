using MediatR;
using TicketManagementSystem.Application.Abstractions.Repositories;
using TicketManagementSystem.Domain.Entities;

namespace TicketManagementSystem.Application.Features.TicketCategories.CreateTicketCategory;

public sealed class CreateTicketCategoryCommandHandler : 
    IRequestHandler<CreateTicketCategoryCommand, CreateTicketCategoryResponse>
{
    private readonly ITicketCategoryRepository _ticketCategoryRepository;

    public CreateTicketCategoryCommandHandler(ITicketCategoryRepository ticketCategoryRepository)
    {
        _ticketCategoryRepository = ticketCategoryRepository;
    }

    public async Task<CreateTicketCategoryResponse> Handle(CreateTicketCategoryCommand request, 
                                        CancellationToken cancellationToken)
    {
        var existed = await _ticketCategoryRepository.ExistsByNameAsync(request.Name, cancellationToken);
        if (existed)
        {
            throw new InvalidOperationException("Ticket category with the same name already exists.");
        }

        var ticketCategory = TicketCategory.Create(request.Name, request.Description);
        await _ticketCategoryRepository.AddAsync(ticketCategory, cancellationToken);

        return new CreateTicketCategoryResponse(
            ticketCategory.Id,
            ticketCategory.Name,
            ticketCategory.Description,
            ticketCategory.IsActive
        );
    }
}