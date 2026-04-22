namespace TicketManagementSystem.Application.Features.TicketCategories.GetTicketCategories;

public sealed record TicketCategoryDto(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive,
    DateTime CreatedAtUtc,
    DateTime? UpdatedAtUtc
);