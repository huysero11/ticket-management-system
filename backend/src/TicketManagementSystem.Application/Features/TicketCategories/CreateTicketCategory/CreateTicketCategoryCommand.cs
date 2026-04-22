using MediatR;

namespace TicketManagementSystem.Application.Features.TicketCategories.CreateTicketCategory
{
    public sealed record CreateTicketCategoryCommand(
        string Name, string? Description) : IRequest<CreateTicketCategoryResponse>;
}