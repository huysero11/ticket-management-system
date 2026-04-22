namespace TicketManagementSystem.Application.Features.TicketCategories.CreateTicketCategory;

public sealed record CreateTicketCategoryResponse(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive
);