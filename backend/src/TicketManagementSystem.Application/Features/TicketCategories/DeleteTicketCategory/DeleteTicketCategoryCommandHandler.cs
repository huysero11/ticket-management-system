using MediatR;
using TicketManagementSystem.Application.Abstractions.Repositories;

namespace TicketManagementSystem.Application.Features.TicketCategories.DeleteTicketCategory;

public sealed class DeleteTicketCategoryCommandHandler : IRequestHandler<DeleteTicketCategoryCommand>
{
    private readonly ITicketCategoryRepository _repository;

    public DeleteTicketCategoryCommandHandler(ITicketCategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteTicketCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = await _repository.GetByIdAsync(command.Id, cancellationToken);
        if (category is null)
        {
            throw new InvalidOperationException($"Ticket category with ID {command.Id} not found.");
        }

        if (!category.IsActive)
        {
            throw new InvalidOperationException($"Ticket category with ID {command.Id} is already inactive.");
        }

        category.Deactivate();
        await _repository.UpdateAsync(category, cancellationToken);
    }
}