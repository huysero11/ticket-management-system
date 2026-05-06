using FluentValidation;

namespace TicketManagementSystem.Application.Features.TicketComments.UpdateTicketComment;

public sealed class UpdateTicketCommentCommandValidator
    : AbstractValidator<UpdateTicketCommentCommand>
{
    public UpdateTicketCommentCommandValidator()
    {
        RuleFor(x => x.TicketId)
            .NotEmpty();

        RuleFor(x => x.CommentId)
            .NotEmpty();

        RuleFor(x => x.Message)
            .NotEmpty()
            .MaximumLength(2000);
    }
}