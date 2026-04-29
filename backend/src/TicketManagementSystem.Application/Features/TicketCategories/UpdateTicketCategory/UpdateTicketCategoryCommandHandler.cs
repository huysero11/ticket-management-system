using MediatR;
using TicketManagementSystem.Application.Abstractions.Repositories;

namespace TicketManagementSystem.Application.Features.TicketCategories.UpdateTicketCategory;

public sealed class UpdateTicketCategoryCommandHandler : IRequestHandler<UpdateTicketCategoryCommand>
{
    private readonly ITicketCategoryRepository _repository;

    public UpdateTicketCategoryCommandHandler(ITicketCategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(UpdateTicketCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (category is null)
        {
            throw new InvalidOperationException($"Ticket category with id {request.Id} not found.");
        }

        var exists = await _repository.ExistsByNameAsync(request.Name, request.Id, cancellationToken);
        if (exists)
        {
            throw new InvalidOperationException($"Ticket category with name {request.Name} already exists.");
        }

        category.Update(request.Name, request.Description);
        await _repository.UpdateAsync(category, cancellationToken);
    }
}