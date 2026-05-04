using FluentValidation;

namespace TicketManagementSystem.Application.Features.TicketCategories.UpdateTicketCategory;

public sealed class UpdateTicketCategoryCommandValidator
    : AbstractValidator<UpdateTicketCategoryCommand>
{
    public UpdateTicketCategoryCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Description)
            .MaximumLength(255);
    }
}