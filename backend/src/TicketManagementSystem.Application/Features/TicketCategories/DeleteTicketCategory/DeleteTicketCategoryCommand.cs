using MediatR;

namespace TicketManagementSystem.Application.Features.TicketCategories.DeleteTicketCategory;

public sealed record DeleteTicketCategoryCommand(Guid Id) : IRequest;