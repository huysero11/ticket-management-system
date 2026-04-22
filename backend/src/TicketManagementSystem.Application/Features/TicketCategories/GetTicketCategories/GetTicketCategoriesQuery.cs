using MediatR;

namespace TicketManagementSystem.Application.Features.TicketCategories.GetTicketCategories;

public sealed record GetTicketCategoriesQuery(bool IncludeInactive = false) : IRequest<IReadOnlyList<TicketCategoryDto>>;