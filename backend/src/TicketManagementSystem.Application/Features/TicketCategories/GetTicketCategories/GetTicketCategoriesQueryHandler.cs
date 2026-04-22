using MediatR;
using TicketManagementSystem.Application.Abstractions.Repositories;
using TicketManagementSystem.Application.Features.TicketCategories.GetTicketCategories;

namespace TicketManagementSystem.Application.Features.TicketCategories.CreateTicketCategory;

public sealed class GetTicketCategoriesQueryHandler : IRequestHandler<GetTicketCategoriesQuery, IReadOnlyList<TicketCategoryDto>>
{
    private readonly ITicketCategoryRepository _ticketCategoryRepository;

    public GetTicketCategoriesQueryHandler(ITicketCategoryRepository ticketCategoryRepository)
    {
        _ticketCategoryRepository = ticketCategoryRepository;
    }

    public async Task<IReadOnlyList<TicketCategoryDto>> Handle(GetTicketCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _ticketCategoryRepository.GetAllAsync(request.IncludeInactive, cancellationToken);
        return categories.Select(c => new TicketCategoryDto(
            c.Id,
            c.Name,
            c.Description,
            c.IsActive,
            c.CreatedAtUtc,
            c.UpdatedAtUtc
        )).ToList();
    }
}