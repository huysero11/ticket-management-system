
using MediatR;
using TicketManagementSystem.Application.Abstractions.Repositories;
using TicketManagementSystem.Application.Features.TicketCategories.GetTicketCategories;

namespace TicketManagementSystem.Application.Features.TicketCategories.GetTicketCategoryById;

public sealed class GetTicketCategoryByIdQueryHandler : IRequestHandler<GetTicketCategoryByIdQuery, TicketCategoryDto>
{
    private readonly ITicketCategoryRepository _ticketCategoryRepository;

    public GetTicketCategoryByIdQueryHandler(ITicketCategoryRepository ticketCategoryRepository)
    {
        _ticketCategoryRepository = ticketCategoryRepository;
    }

    public async Task<TicketCategoryDto> Handle(GetTicketCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _ticketCategoryRepository.GetByIdAsync(request.Id, cancellationToken);
        if (category is null)
        {
            throw new KeyNotFoundException($"Ticket category with ID {request.Id} not found.");
        }

        return new TicketCategoryDto(
            category.Id,
            category.Name,
            category.Description,
            category.IsActive,
            category.CreatedAtUtc,
            category.UpdatedAtUtc
        );
    }
}