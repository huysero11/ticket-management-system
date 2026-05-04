using FluentValidation;

namespace TicketManagementSystem.Application.Features.Tickets.UpdateTicket;

public sealed class UpdateTicketCommandValidator
    : AbstractValidator<UpdateTicketCommand>
{
    public UpdateTicketCommandValidator()
    {
        RuleFor(x => x.TicketId)
            .NotEmpty();

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