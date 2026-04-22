using MediatR;
using TicketManagementSystem.Application.Features.TicketCategories.GetTicketCategories;

namespace TicketManagementSystem.Application.Features.TicketCategories.GetTicketCategoryById;

public sealed record GetTicketCategoryByIdQuery(Guid Id) : IRequest<TicketCategoryDto>;