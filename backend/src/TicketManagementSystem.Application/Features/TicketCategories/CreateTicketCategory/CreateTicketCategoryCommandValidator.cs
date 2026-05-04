using FluentValidation;

namespace TicketManagementSystem.Application.Features.TicketCategories.CreateTicketCategory;

public sealed class CreateTicketCategoryCommandValidator
    : AbstractValidator<CreateTicketCategoryCommand>
{
    public CreateTicketCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Description)
            .MaximumLength(255);
    }
}