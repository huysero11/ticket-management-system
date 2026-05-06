using FluentValidation;

namespace TicketManagementSystem.Application.Features.TicketComments.AddTicketComment;

public sealed class AddTicketCommentCommandValidator
    : AbstractValidator<AddTicketCommentCommand>
{
    public AddTicketCommentCommandValidator()
    {
        RuleFor(x => x.TicketId)
            .NotEmpty();

        RuleFor(x => x.Message)
            .NotEmpty()
            .MaximumLength(2000);
    }
}