using FluentValidation;

namespace TicketManagementSystem.Application.Features.TicketComments.DeleteTicketComment;

public sealed class DeleteTicketCommentCommandValidator
    : AbstractValidator<DeleteTicketCommentCommand>
{
    public DeleteTicketCommentCommandValidator()
    {
        RuleFor(x => x.TicketId)
            .NotEmpty();

        RuleFor(x => x.CommentId)
            .NotEmpty();
    }
}