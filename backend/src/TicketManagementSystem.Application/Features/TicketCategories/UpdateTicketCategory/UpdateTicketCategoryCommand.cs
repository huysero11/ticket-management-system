using MediatR;

namespace TicketManagementSystem.Application.Features.TicketCategories.UpdateTicketCategory;

public sealed record UpdateTicketCategoryCommand(
    Guid Id,
    string Name,
    string? Description
) : IRequest;