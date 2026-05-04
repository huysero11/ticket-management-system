using FluentValidation;

namespace TicketManagementSystem.Application.Features.Tickets.CreateTicket;

public sealed class CreateTicketCommandValidator
    : AbstractValidator<CreateTicketCommand>
{
    public CreateTicketCommandValidator()
    {
        /*
        we have this line in ValidationBehavior: 
            var context = new ValidationContext<TRequest>(request); 
        context includes the object that need to be validated,
        and x is the request in that context
        */
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(2000);

        RuleFor(x => x.CategoryId)
            .NotEmpty();

        RuleFor(x => x.Priority)
            .IsInEnum();
    }
}